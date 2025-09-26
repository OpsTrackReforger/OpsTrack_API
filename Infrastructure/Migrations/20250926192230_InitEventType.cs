using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitEventType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    eventTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.eventTypeId);
                });

            // Seed initial data
            migrationBuilder.InsertData(
                table: "EventType",
                columns: new[] { "eventTypeId", "name", "category", "description" },
                values: new object[,]
                {
                    { 1, "Kill", "Combat", "Entity killed another entity" },
                    { 2, "Wounded", "Combat", "Entity wounded another entity" },
                    { 3, "Join", "Player", "Player joined the server" },
                    { 4, "Leave", "Player", "Player left the server" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventType");
        }
    }
}
