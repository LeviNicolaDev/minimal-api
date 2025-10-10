using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;

namespace Test.Domain.Services;

[TestClass]
public sealed class AdministratorServiceTest
{
    private DbContexto? _context;
    private DbContexto CreateContextTest()
    {
        // 1. Cria opções do DbContext usando o provedor In-Memory
        // O nome do banco de dados (ex: Guid.NewGuid().ToString()) garante isolamento
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // 2. Passa as opções para o construtor do DbContexto
        return new DbContexto(options);
    }

    [TestInitialize]
    public void TestInitialize()
    {
        // 1. Cria o contexto de teste
        _context = CreateContextTest();
        // 2. Não há necessidade de iniciar transação, pois o In-Memory já é isolado por teste
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // Limpa o banco de dados em memória após cada teste
        _context?.Database.EnsureDeleted();
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