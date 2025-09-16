using Microsoft.EntityFrameworkCore;
using OpsTrack_API.Models;

namespace OpsTrack_API.Data
{
    public class OpsTrackContext : DbContext
    {
        public OpsTrackContext(DbContextOptions<OpsTrackContext> options) : base(options) { }
        public DbSet<PlayerEvent> PlayerEvents => Set<PlayerEvent>();

    }
}
