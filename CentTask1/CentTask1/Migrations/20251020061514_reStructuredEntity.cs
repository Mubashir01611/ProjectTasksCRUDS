using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CentTask1.Migrations
{
    /// <inheritdoc />
    public partial class reStructuredEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "id",
                table: "ProjectTasks");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "ProjectTasks",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ProjectTasks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ProjectTasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "ProjectTasks",
                newName: "UpdatedOn");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "ProjectTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ProjectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectTasks");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "ProjectTasks",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProjectTasks",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ProjectTasks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "ProjectTasks",
                newName: "DueDate");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTasks",
                table: "ProjectTasks",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
