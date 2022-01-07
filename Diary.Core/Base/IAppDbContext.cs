using Diary.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.Core.Base;

public interface IAppDbContext
{
    public DbSet<DiaryItem> DiaryItems { get; }
    public DbSet<ItemType> ItemTypes { get; }
    public DbSet<RedMineActivity> RedMineActivities { get; }
    public DbSet<RedMineIssue> RedMineIssues { get; }
}