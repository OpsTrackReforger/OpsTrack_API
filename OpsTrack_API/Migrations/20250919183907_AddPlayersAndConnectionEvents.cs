using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpsTrack_API.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayersAndConnectionEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ConnectionEvents",
                newName: "EventId");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    GameIdentity = table.Column<string>(type: "TEXT", nullable: false),
                    LastKnownName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.GameIdentity);
                });

            migrationBuilder.Sql(@"
                DELETE FROM ConnectionEvents
                WHERE GameIdentity NOT IN (SELECT GameIdentity FROM Players)
                ");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionEvents_GameIdentity",
                table: "ConnectionEvents",
                column: "GameIdentity");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionEvents_Players_GameIdentity",
                table: "ConnectionEvents",
                column: "GameIdentity",
                principalTable: "Players",
                principalColumn: "GameIdentity",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionEvents_Players_GameIdentity",
                table: "ConnectionEvents");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropIndex(
                name: "IX_ConnectionEvents_GameIdentity",
                table: "ConnectionEvents");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "ConnectionEvents",
                newName: "Id");
        }
    }
}
