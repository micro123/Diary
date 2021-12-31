using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diary.Core.Entities;

public class DiaryItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; }
    public string Note { get; set; }
    public ItemType Type { get; set; }

    // RedMine 相关
    public RedMineIssue? Issue { get; set; }
    public RedMineActivity? Activity { get; set; }
}