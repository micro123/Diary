using System;
using Prism.Services.Dialogs;

namespace Diary.App.Modules.RedMine.Dialogs;

public class RedMineIssueImportViewModel: IDialogAware
{
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    public string Title { get; } = "导入RedMine问题";
    public event Action<IDialogResult>? RequestClose;
}