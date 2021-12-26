using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Core;

namespace Diary.App;

public class DiaryApp: IApplication
{
    public ObservableCollection<MenuNode> MenuNodes { get; } = new();

    public void RegisterMenu(MenuNode? menuNode)
    {
        if (menuNode.HasValue)
            MenuNodes.Add(menuNode.Value);
    }

    public ObservableCollection<MenuNode> Menus => MenuNodes;


}