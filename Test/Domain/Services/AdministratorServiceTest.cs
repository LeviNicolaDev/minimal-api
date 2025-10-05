using System.Reflection;
using Microsoft.Extensions.Configuration;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore.Storage;

namespace Test.Domain.Services;

[TestClass]
public sealed class AdministratorServiceTest
{
    private DbContexto? _context;
    private IDbContextTransaction? _transaction;
    
    private DbContexto CreateContextTest()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DbContexto(configuration);
    }

    [TestInitialize]
    public void TestInitialize()
    {
        // 1. Cria o contexto de teste
        _context = CreateContextTest();
        // 2. Inicia uma transação antes de cada teste.
        // Isso garante que todas as operações de banco de dados neste teste possam ser revertidas.
        _transaction = _context.Database.BeginTransaction();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // No final de cada teste, faz o rollback da transação.
        // Isso garante que os dados inseridos (o Adm) sejam removidos do banco,
        // limpando o estado para o próximo teste, mesmo em paralelismo.
        _transaction?.Rollback();
        _transaction?.Dispose();
        _context?.Dispose();
    }
    
    [TestMethod]
    public void SaveAdmTest()
    {
        // Arrange
        var adm = new Administrator();
        adm.Email = "teste@teste.com";
        adm.Senha = "123456";
        adm.Perfil = "Adm";
        
        var administratorService = new AdministratorService(_context!);
        
        // Action
        administratorService.Include(adm);
        
        
        // Assert
        Assert.AreEqual(1, _context!.Administrators.Count());
    }

    [TestMethod]
    public void AdmSearchByIdTest()
    {
        // Arrange
        var adm = new Administrator();
        adm.Email = "teste2@teste.com";
        adm.Senha = "789012";
        adm.Perfil = "Adm";
        
        var administratorService = new AdministratorService(_context!);
        
        // Action
        administratorService.Include(adm);
        var admOfDatabase = administratorService.SearchById(adm.Id);
        
        // Assert
        Assert.IsNotNull(admOfDatabase);
        Assert.AreEqual(adm.Email, admOfDatabase.Email);
        Assert.AreEqual(adm.Id, admOfDatabase.Id);
    }
}