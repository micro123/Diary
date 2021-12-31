using System.ComponentModel.DataAnnotations;

namespace Diary.App.Database;

public class RedMineIssue
{
    [Key]
    public int IssueId { get; set; }
    public string IssueName { get; set; }
}