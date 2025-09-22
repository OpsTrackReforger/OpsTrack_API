using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpsTrack_API.Migrations
{
    /// <inheritdoc />
    public partial class RenamePlayerEventsToConnectionEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PlayerEvents",
                newName: "ConnectionEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ConnectionEvents",
                newName: "PlayerEvents");
        }

    }
}
