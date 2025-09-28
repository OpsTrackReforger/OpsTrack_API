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

        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}
