using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentTask1.Migrations
{
    /// <inheritdoc />
    public partial class removedproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "ProjectTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "ProjectTasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
