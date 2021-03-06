using Diary.Core.Base;
using Diary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Diary.App.Database;

public class DiaryDbContext : DbContext, IAppDbContext
{
    public DbSet<DiaryItem> DiaryItems { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<RedMineActivity> RedMineActivities { get; set; }
    public DbSet<RedMineIssue> RedMineIssues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appName = System.AppDomain.CurrentDomain.FriendlyName;
        optionsBuilder.UseSqlite($@"Data Source={appDataDir}\{appName}\Diary.db;");
    }
}