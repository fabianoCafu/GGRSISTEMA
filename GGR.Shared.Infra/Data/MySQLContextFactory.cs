using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GGR.Shared.Infra.Data
{
    public class MySQLContextFactory
        : IDesignTimeDbContextFactory<MySQLContext>
    {
        public MySQLContext CreateDbContext(string[] args)
        {

            //var urlBase = ConfigurationBuilder.Configuration["ApiSettings:UrlBase"];


            //var configuration = new ConfigurationBuilder();
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: false)
            //.Build();

            //var connectionString = configuration
            //.GetConnectionString("MySQLConnectionString");


            //var configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json")
            //.Build();

            //var connection = configuration.GetConnectionString("MySQLConnectionString");

            //server=127.0.0.1
            var connection = "server=127.0.0.1;Port=3308;database=GGRDataBase;user=root;password=adm123;";
            var options = new DbContextOptionsBuilder<MySQLContext>().UseMySql(connection, ServerVersion.AutoDetect(connection)).Options;

            return new MySQLContext(options);
        }
    }
}
