using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class postlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskLists_TaskCollectionId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "TaskCollectionId",
                table: "Tasks",
                newName: "TaskListId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskCollectionId",
                table: "Tasks",
                newName: "IX_Tasks_TaskListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskLists_TaskListId",
                table: "Tasks",
                column: "TaskListId",
                principalTable: "TaskLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskLists_TaskListId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "TaskListId",
                table: "Tasks",
                newName: "TaskCollectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskListId",
                table: "Tasks",
                newName: "IX_Tasks_TaskCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskLists_TaskCollectionId",
                table: "Tasks",
                column: "TaskCollectionId",
                principalTable: "TaskLists",
                principalColumn: "Id");
        }
    }
}
