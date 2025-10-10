using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;
// REMOVA: using Microsoft.Extensions.Configuration; // Não é mais necessário aqui

namespace minimal_api.Infrastructure.Db;

public class DbContexto : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContexto(DbContextOptions<DbContexto> options) : base(options)
    {
    }
    
    public DbSet<Administrator> Administrators { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>().HasData(
            new Administrator
            {
                Id = 1,
                Email = "administrador@teste.com",
                Senha = "123456", // TODO: criptografar a senha
                Perfil = "Adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var stringConexao = new ConfigurationBuilder() 
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .Build()
                .GetConnectionString("AzureConnection");

            if (!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseSqlServer(stringConexao);
            }
        }
    }
}