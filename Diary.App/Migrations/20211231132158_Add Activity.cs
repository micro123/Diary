using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diary.App.Migrations
{
    public partial class AddActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "DiaryItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiaryItems_ActivityId",
                table: "DiaryItems",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaryItems_RedMineActivities_ActivityId",
                table: "DiaryItems",
                column: "ActivityId",
                principalTable: "RedMineActivities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaryItems_RedMineActivities_ActivityId",
                table: "DiaryItems");

            migrationBuilder.DropIndex(
                name: "IX_DiaryItems_ActivityId",
                table: "DiaryItems");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "DiaryItems");
        }
    }
}
