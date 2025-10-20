using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentTask1.Migrations
{
    /// <inheritdoc />
    public partial class updatedIDname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "ProjectTasks",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProjectTasks",
                newName: "TaskId");
        }
    }
}
