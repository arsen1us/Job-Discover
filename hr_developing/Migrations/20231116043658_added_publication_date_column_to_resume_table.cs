using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr_developing.Migrations
{
    /// <inheritdoc />
    public partial class added_publication_date_column_to_resume_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "publication_time",
                table: "resumes",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "publication_date",
                table: "resumes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "publication_date",
                table: "resumes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "publication_time",
                table: "resumes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
