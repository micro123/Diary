using Diary.App.Models;
using Diary.App.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
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

    private ICommand? _updateRedMineCookiesCommand;

    public ICommand UpdateRedMineCookiesCommand =>
        _updateRedMineCookiesCommand ??=
            new DelegateCommand<object>((passwordBox) => UpdateRedMineCookies(passwordBox), CanDoUpdate)
                .ObservesProperty(() => Host)
                .ObservesProperty(() => Port)
                .ObservesProperty(() => User)
                .ObservesProperty(() => Updating);

    private bool CanDoUpdate(object passwordBox)
    {
        return !Updating && !string.IsNullOrWhiteSpace(Host) && !string.IsNullOrWhiteSpace(User) && Port is >= 0 and <= 65535;
    }

    private async Task UpdateRedMineCookies(object passwordBox)
    {
        Updating = true;
        SaveData();
        if (passwordBox is PasswordBox box)
        {
            var pwd = box.Password;
            if (RedMineUtility.UpdateCookie(pwd).Result)
            {

            }
            else
            {

            }
        }

        Updating = false;
    }

    private ICommand? _recheckCanExecuteCommand;

    public ICommand RecheckCanExecuteCommand =>
        _recheckCanExecuteCommand ??= new DelegateCommand<object>(RecheckCanExecute);

    private void RecheckCanExecute(object obj)
    {
        if (obj is PasswordBox passwordBox)
        {
            (UpdateRedMineCookiesCommand as DelegateCommand<object>)?.RaiseCanExecuteChanged();
        }
    }

    private void RestorePreviousSettings()
    {
        LoadData();
    }

    private void LoadData()
    {
        var d = AppSettings.Load();
        Host = d.RedMineHost;
        Port = d.RedMinePort;
        User = d.RedMineUser;
    }

    private void SaveData()
    {
        AppSettings.Store(new AppSettings.Data()
        {
            RedMineHost = Host,
            RedMinePort = Port,
            RedMineUser = User,
            RedMineCookies = AppSettings.GetConfig().RedMineCookies
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

    private string _user = "";
    public string User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    private bool _updating = false;

    public bool Updating
    {
        get => _updating;
        set => SetProperty(ref _updating, value);
    }
}