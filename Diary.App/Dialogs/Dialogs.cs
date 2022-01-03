using Diary.App.Dialogs.Content;
using Diary.App.Dialogs.VM;
using Prism.Ioc;

namespace Diary.App.Dialogs;

public class Dialogs
{
    public static void ConfigAppDialogs(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterDialog<SettingsView, SettingsViewModel>("SettingsDialog");
    }
}