using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr_developing.Migrations
{
    /// <inheritdoc />
    public partial class drom_last_added_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publication_date",
                table: "resumes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "publication_date",
                table: "resumes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
