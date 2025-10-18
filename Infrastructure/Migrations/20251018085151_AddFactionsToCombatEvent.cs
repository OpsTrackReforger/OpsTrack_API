using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionsToCombatEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActorFaction",
                table: "CombatEvent",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                defaultValue: "Unknown"
                );

            migrationBuilder.AddColumn<string>(
                name: "VictimFaction",
                table: "CombatEvent",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                defaultValue: "Unknown"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActorFaction",
                table: "CombatEvent");

            migrationBuilder.DropColumn(
                name: "VictimFaction",
                table: "CombatEvent");
        }
    }
}
