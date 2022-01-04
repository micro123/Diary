using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diary.Core.Base;

public struct AppMenuItem
{
    public string Title { get; set; } = string.Empty;

    public ICommand? Command { get; set; } = null;

    public Canvas? Icon { get; set; }
}