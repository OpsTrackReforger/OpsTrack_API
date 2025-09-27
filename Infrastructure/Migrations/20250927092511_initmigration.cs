using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
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
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.eventTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    GameIdentity = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastKnownName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    FirstSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.GameIdentity);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_EventType_EventTypeId",
                        column: x => x.EventTypeId,
                        principalTable: "EventType",
                        principalColumn: "eventTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CombatEvent",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActorId = table.Column<string>(type: "TEXT", nullable: false),
                    VictimId = table.Column<string>(type: "TEXT", nullable: false),
                    Weapon = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Distance = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTeamKill = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombatEvent", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_CombatEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CombatEvent_Player_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Player",
                        principalColumn: "GameIdentity",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CombatEvent_Player_VictimId",
                        column: x => x.VictimId,
                        principalTable: "Player",
                        principalColumn: "GameIdentity",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionEvent",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameIdentity = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionEvent", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_ConnectionEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConnectionEvent_Player_GameIdentity",
                        column: x => x.GameIdentity,
                        principalTable: "Player",
                        principalColumn: "GameIdentity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CombatEvent_ActorId",
                table: "CombatEvent",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_CombatEvent_VictimId",
                table: "CombatEvent",
                column: "VictimId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionEvent_GameIdentity",
                table: "ConnectionEvent",
                column: "GameIdentity");

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventTypeId",
                table: "Event",
                column: "EventTypeId");


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
                name: "CombatEvent");

            migrationBuilder.DropTable(
                name: "ConnectionEvent");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "EventType");
        }
    }
}
