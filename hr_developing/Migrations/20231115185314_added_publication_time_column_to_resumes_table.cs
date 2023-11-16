using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr_developing.Migrations
{
    /// <inheritdoc />
    public partial class added_publication_time_column_to_resumes_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<DateTime>(
                name: "publication_time",
                table: "resumes",
                type: "time",
                nullable: false,
                defaultValue: DateTime.Now);
            

        }
    }
}
