using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Diary.App.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedMineActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedMineActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedMineIssues",
                columns: table => new
                {
                    IssueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IssueName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedMineIssues", x => x.IssueId);
                });

            migrationBuilder.CreateTable(
                name: "DiaryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    IssueId = table.Column<int>(type: "INTEGER", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryItems_ItemTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiaryItems_RedMineIssues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "RedMineIssues",
                        principalColumn: "IssueId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryItems_IssueId",
                table: "DiaryItems",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryItems_TypeId",
                table: "DiaryItems",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryItems");

            migrationBuilder.DropTable(
                name: "RedMineActivities");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "RedMineIssues");
        }
    }
}