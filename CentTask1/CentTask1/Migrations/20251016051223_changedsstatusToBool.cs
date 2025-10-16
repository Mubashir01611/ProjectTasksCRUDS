using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentTask1.Migrations
{
    /// <inheritdoc />
    public partial class changedsstatusToBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
          @"UPDATE Projects SET Status = '1' WHERE Status = 'active';
              UPDATE Projects SET Status = '0' WHERE Status = 'inactive';"
      );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
             @"UPDATE Projects SET Status = 'active' WHERE Status = '1';
              UPDATE Projects SET Status = 'inactive' WHERE Status = '0';"
         );
        }
    }
}
