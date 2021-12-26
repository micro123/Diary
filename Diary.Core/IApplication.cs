using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Diary.Core;

public interface IApplication
{
    public ObservableCollection<MenuNode> Menus { get; }
    void RegisterMenu(MenuNode? menuNode); // 向程序注册菜单项
}