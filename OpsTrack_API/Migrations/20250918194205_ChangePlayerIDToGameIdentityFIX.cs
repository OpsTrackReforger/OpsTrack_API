using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpsTrack_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlayerIDToGameIdentityFIX : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameIdentity",
                table: "ConnectionEvents",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameIdentity",
                table: "ConnectionEvents",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
