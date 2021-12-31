using Microsoft.EntityFrameworkCore;

namespace Diary.App.Database;

public class DiaryDbContext : DbContext
{
    public DbSet<DiaryItem> DiaryItems { get; set; }
    public DbSet<RedMineIssue> RedMineIssues { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        // optionsBuilder.UseSqlite($@"Data Source={appDataDir}\mydb.db;Version=3;");
        optionsBuilder.UseSqlite(@"Data Source=E:\DB\Diary.db");
    }
}