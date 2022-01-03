using System.Collections;
using System.Collections.Generic;
using Diary.Core.Base;
using System.Collections.ObjectModel;

namespace Diary.App;

public class DiaryApp : IApplication
{
    public ObservableCollection<AppMenuItem> MenuItems { get; } = new();

    public void RegisterMenu(IEnumerable<AppMenuItem>? menuNode)
    {
        if (menuNode != null)
            MenuItems.AddRange(menuNode);
    }

    public ObservableCollection<AppMenuItem> Menus => MenuItems;


}