using System.Collections.Generic;
using System.Windows.Input;

namespace Diary.Core.Base;

public struct MenuNode
{
    public string Title { get; set; } = string.Empty;
    public ICommand? Command { get; set; } = null;

    public ICollection<MenuNode> Children { get; set; } = new List<MenuNode>();
}