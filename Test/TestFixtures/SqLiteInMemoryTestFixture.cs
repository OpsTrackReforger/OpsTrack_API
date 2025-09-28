using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Test.TestFixtures
{
    public class SqLiteInMemoryTestFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        public OpsTrackContext Context { get; }

        public SqLiteInMemoryTestFixture()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<OpsTrackContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new OpsTrackContext(options);
            Context.Database.EnsureCreated();
        }
        public OpsTrackContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<OpsTrackContext>()
                .UseSqlite(_connection)
                .Options;

            var context = new OpsTrackContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public OpsTrackContext CreateIsolatedContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<OpsTrackContext>()
                .UseSqlite(connection)
                .Options;

            var context = new OpsTrackContext(options);
            context.Database.EnsureCreated();
            return context;
        }



        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
