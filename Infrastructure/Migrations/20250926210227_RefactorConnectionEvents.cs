using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorConnectionEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Events (EventId, TimeStamp, EventTypeId)
                SELECT 
                    ce.EventId,
                    ce.Timestamp,
                    (SELECT et.eventTypeId 
                     FROM EventType et 
                     WHERE lower(et.name) = lower(ce.EventType)
                     LIMIT 1)
                FROM ConnectionEvents ce
                WHERE NOT EXISTS (
                    SELECT 1 FROM Events e WHERE e.EventId = ce.EventId
                );
            ");


            migrationBuilder.DropColumn(
                name: "EventType",
                table: "ConnectionEvents");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "ConnectionEvents");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "ConnectionEvents",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionEvents_Events_EventId",
                table: "ConnectionEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionEvents_Events_EventId",
                table: "ConnectionEvents");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "ConnectionEvents",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "ConnectionEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "ConnectionEvents",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
