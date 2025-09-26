using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;

namespace minimal_api.Infrastructure.Db;

public class DbContexto : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configurationAppSettings;

    public DbContexto(IConfiguration configurationAppSettings)
    {
        _configurationAppSettings = configurationAppSettings;
    }
    
    
    public DbSet<Administrator> Administrators { get; set; } = default!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var stringConexao = _configurationAppSettings.GetConnectionString("MySql")?.ToString();
            if (!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseMySql(stringConexao, 
                    ServerVersion.AutoDetect(stringConexao));
            }
        }
        
    }
}