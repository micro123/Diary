using System.ComponentModel.DataAnnotations;

namespace Diary.Core.Entities;

public class RedMineIssue
{
    [Key]
    public int IssueId { get; set; }

    public string IssueName { get; set; }
}