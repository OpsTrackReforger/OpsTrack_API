using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace Infrastructure.Data
{
    public class OpsTrackContextFactory : IDesignTimeDbContextFactory<OpsTrackContext>
    {
        public OpsTrackContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpsTrackContext>();
            optionsBuilder.UseSqlite("Data Source=opstrack.db");

            return new OpsTrackContext(optionsBuilder.Options);
        }
    }
}
