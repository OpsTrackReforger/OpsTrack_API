using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCombatEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CombatEvent",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActorId = table.Column<string>(type: "TEXT", nullable: true),
                    VictimId = table.Column<string>(type: "TEXT", nullable: true),
                    Weapon = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Distance = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTeamKill = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatEvent", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_CombatEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CombatEvent_Players_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Players",
                        principalColumn: "GameIdentity",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CombatEvent_Players_VictimId",
                        column: x => x.VictimId,
                        principalTable: "Players",
                        principalColumn: "GameIdentity",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CombatEvent_ActorId",
                table: "CombatEvent",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatEvent_VictimId",
                table: "CombatEvent",
                column: "VictimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CombatEvent");
        }
    }
}
