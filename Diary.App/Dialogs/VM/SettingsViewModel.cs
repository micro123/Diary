using Diary.App.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace Diary.App.Dialogs.VM;

public class SettingsViewModel : BindableBase, IDialogAware
{
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        LoadData();
    }

    public string Title => "设置";

    public event Action<IDialogResult>? RequestClose;

    private ICommand? _saveAndCloseCommand;
    public ICommand SaveAndCloseCommand => _saveAndCloseCommand ??= new DelegateCommand(SaveSettingsAndClose);

    private void SaveSettingsAndClose()
    {
        SaveData();
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }

    private ICommand? _cancelAndCloseCommand;
    public ICommand CancelAndCloseCommand => _cancelAndCloseCommand ??= new DelegateCommand(CancelChangesAndClose);

    private void CancelChangesAndClose()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    private ICommand? _restoreSettingsCommand;
    public ICommand RestoreSettingsCommand => _restoreSettingsCommand ??= new DelegateCommand(RestorePreviousSettings);

    private void RestorePreviousSettings()
    {
        LoadData();
    }

    private void LoadData()
    {
        var d = AppSettings.Load();
        Host = d.RedMineHost;
        Port = d.RedMinePort;
        _userApiKey = d.RedMineUserApiKey;
    }

    private void SaveData()
    {
        AppSettings.Store(new AppSettings.Data()
        {
            RedMineHost = Host,
            RedMinePort = Port,
            RedMineUserApiKey = _userApiKey
        });
    }

    private string _host = "";

    public string Host
    {
        get => _host;
        set => SetProperty(ref _host, value);
    }

    private int _port;

    public int Port
    {
        get => _port;
        set => SetProperty(ref _port, value);
    }

    private string _userApiKey = "";

    public string UserApiKey
    {
        get => _userApiKey;
        set => SetProperty(ref _userApiKey, value);
    }

    private bool _updating = false;

    public bool Updating
    {
        get => _updating;
        set => SetProperty(ref _updating, value);
    }
}