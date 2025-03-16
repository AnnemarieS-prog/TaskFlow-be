using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskCollections_TaskCollectionId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskCollections",
                table: "TaskCollections");

            migrationBuilder.RenameTable(
                name: "TaskCollections",
                newName: "TaskCollection");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskCollection",
                table: "TaskCollection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskCollection_TaskCollectionId",
                table: "Tasks",
                column: "TaskCollectionId",
                principalTable: "TaskCollection",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskCollection_TaskCollectionId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskCollection",
                table: "TaskCollection");

            migrationBuilder.RenameTable(
                name: "TaskCollection",
                newName: "TaskCollections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskCollections",
                table: "TaskCollections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskCollections_TaskCollectionId",
                table: "Tasks",
                column: "TaskCollectionId",
                principalTable: "TaskCollections",
                principalColumn: "Id");
        }
    }
}
