using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanDeck.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerClientId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClientUserId",
                table: "Participants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "Participants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerClientId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ClientUserId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "Participants");
        }
    }
}
