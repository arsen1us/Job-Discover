using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr_developing.Migrations
{
    /// <inheritdoc />
    public partial class added_again_this_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "publication_date",
                table: "resumes",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publication_date",
                table: "resumes");
        }
    }
}
