using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Diary.Core.Base;

public interface IApplication
{
    public ObservableCollection<AppMenuItem> Menus { get; }
    void RegisterMenu(IEnumerable<AppMenuItem>? menuNode); // 向程序注册菜单项
}