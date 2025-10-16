using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentTask1.Migrations
{
    /// <inheritdoc />
    public partial class statusTypeUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
          name: "Status",
          table: "Projects",
          type: "bit",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
           name: "Status",
           table: "Projects",
           type: "nvarchar(max)",
           nullable: true,
           oldClrType: typeof(bool),
           oldType: "bit",
           oldNullable: true);
        }
    }
}
