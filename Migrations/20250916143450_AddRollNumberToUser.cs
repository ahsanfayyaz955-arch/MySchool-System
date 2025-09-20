using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySchool_System.Migrations
{
    /// <inheritdoc />
    public partial class AddRollNumberToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RollNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollNumber",
                table: "AspNetUsers");
        }
    }
}
