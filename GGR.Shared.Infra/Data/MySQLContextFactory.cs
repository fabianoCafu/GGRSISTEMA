using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GR.Shared.Infra.Data
{
    public class MySQLContextFactory 
        : IDesignTimeDbContextFactory<MySQLContext>
    {
        public MySQLContext CreateDbContext(string[] args)
        {
            var connection = "server=localhost;Port=3306;database=GGRDataBase;user=root;password=root;";
            var options = new DbContextOptionsBuilder<MySQLContext>().UseMySql(connection, ServerVersion.AutoDetect(connection)).Options;

            return new MySQLContext(options);
        }
    }
}
