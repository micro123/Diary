using System;

namespace Diary.Core;

public struct TimeEntry
{
    public DateTime CreateTime { get; set; } // 哪一天
    public string Comment { get; set; } // 活动内容
    public double TimeConsume { get; set; } // 耗时
    public int Tag { get; set; } // 自定义标记，可以用来统计
    public int Type { get; set; } // 属于哪一种分类（设计、开发之类的）
    public int Issue { get; set; } // RedMine Issue ID
    public string Note { get; set; } // 详细备注
}
