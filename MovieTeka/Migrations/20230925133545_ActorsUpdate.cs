using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTeka.Migrations
{
    /// <inheritdoc />
    public partial class ActorsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Actors",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Actors");
        }
    }
}
