using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpsTrack_API.Migrations
{
    public partial class ChangePlayerIDToGameIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Først rename
            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "ConnectionEvents",
                newName: "GameIdentity");

            // Så ændre typen fra int til string
            migrationBuilder.AlterColumn<string>(
                name: "GameIdentity",
                table: "ConnectionEvents",
                type: "nvarchar(max)", // eller nvarchar(450) hvis du vil kunne indeksere
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Skift tilbage til int
            migrationBuilder.AlterColumn<int>(
                name: "GameIdentity",
                table: "ConnectionEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Og rename tilbage
            migrationBuilder.RenameColumn(
                name: "GameIdentity",
                table: "ConnectionEvents",
                newName: "PlayerId");
        }
    }
}
