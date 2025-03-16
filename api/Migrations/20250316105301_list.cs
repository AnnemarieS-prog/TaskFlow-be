using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class list : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskCollections_TaskCollectionId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskCollections");

            migrationBuilder.CreateTable(
                name: "TaskLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskLists", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskLists_TaskCollectionId",
                table: "Tasks",
                column: "TaskCollectionId",
                principalTable: "TaskLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskLists_TaskCollectionId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskLists");

            migrationBuilder.CreateTable(
                name: "TaskCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCollections", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskCollections_TaskCollectionId",
                table: "Tasks",
                column: "TaskCollectionId",
                principalTable: "TaskCollections",
                principalColumn: "Id");
        }
    }
}
