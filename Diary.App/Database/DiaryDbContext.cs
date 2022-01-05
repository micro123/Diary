using System;
using System.Reflection;
using Diary.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.App.Database;

public class DiaryDbContext : DbContext
{
    public DbSet<DiaryItem> DiaryItems { get; set; }
    public DbSet<RedMineIssue> RedMineIssues { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<RedMineActivity> RedMineActivities { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appName = System.AppDomain.CurrentDomain.FriendlyName;
        optionsBuilder.UseSqlite($@"Data Source={appDataDir}\{appName}\Diary.db;");
    }
}