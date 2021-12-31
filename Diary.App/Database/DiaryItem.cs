using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary.App.Database;

public class DiaryItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; }
    public RedMineIssue? Issue { get; set; }
    public string Note { get; set; }
    public ItemType Type { get; set; }
}