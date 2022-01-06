using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diary.App.Database;
using Diary.App.Events;
using Diary.App.Utilities;
using Diary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Diary.App.Modules.RedMine.ViewModels;

public class RedMineSettingViewModel : BindableBase
{
    private readonly DiaryDbContext _dbContext;
    private readonly IDialogService _dialogService;

    public RedMineSettingViewModel(DiaryDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        RedMineIssues.CollectionChanged += (sender, args) => { RaisePropertyChanged(nameof(RedMineIssues)); };
        RedMineActivities.CollectionChanged += (sender, args) => { RaisePropertyChanged(nameof(RedMineActivities)); };

        Task.Run(() =>
        {
            LoadActivities();
            LoadIssues();
        });
    }

    private ObservableCollection<RedMineActivity> _redMineActivities = new ObservableCollection<RedMineActivity>();

    public ObservableCollection<RedMineActivity> RedMineActivities
    {
        get => _redMineActivities;
        set => SetProperty(ref _redMineActivities, value);
    }

    private ObservableCollection<RedMineIssue> _redMineIssues = new ObservableCollection<RedMineIssue>();

    public ObservableCollection<RedMineIssue> RedMineIssues
    {
        get => _redMineIssues;
        set => SetProperty(ref _redMineIssues, value);
    }

    private bool _isSyncing = false;

    public bool IsSyncing
    {
        get => _isSyncing;
        private set => SetProperty(ref _isSyncing, value);
    }

    #region Commands

    private ICommand? _importIssueCommand;

    public ICommand ImportIssueCommand =>
        _importIssueCommand ??= new DelegateCommand(ShowImportDialog);

    private void ShowImportDialog()
    {
        _dialogService.ShowDialog("RedMineIssueImport", result =>
        {});
        LoadIssues();
    }

    private ICommand? _deleteIssueCommand;

    public ICommand DeleteIssueCommand =>
        _deleteIssueCommand ??= new DelegateCommand(DeleteSelectedIssue, () => SelectedRedMineIssue != null)
            .ObservesProperty(() => SelectedRedMineIssue);

    private void DeleteSelectedIssue()
    {
        if (_selectedRedMineIssue != null)
        {
            bool confirm = false;
            _dialogService.ShowDialog("MessageDialog", new DialogParameters("desc=确定要删除吗？此操作不可撤销！"),
                result => { confirm = (result.Result == ButtonResult.Yes); }, "msg");
            if (confirm)
            {
                _dbContext.RedMineIssues.Remove(_selectedRedMineIssue);
                SelectedRedMineIssue = null;
                Task.Run(async () =>
                {
                    await _dbContext.SaveChangesAsync();
                    LoadIssues();
                });
            }
        }
    }

    private ICommand? _clearIssuesCommand;

    public ICommand ClearIssuesCommand =>
        _clearIssuesCommand ??=
            new DelegateCommand(ClearIssues, CanExecuteClearIssues).ObservesProperty(
                () => RedMineIssues);

    private bool CanExecuteClearIssues()
    {
        return RedMineIssues.Count > 0;
    }

    private void ClearIssues()
    {
        bool confirmed = false;
        _dialogService.ShowDialog("MessageDialog", new DialogParameters("desc=确定要删除吗？此操作不可撤销！"), result =>
        {
            if (result.Result == ButtonResult.Yes)
                confirmed = true;
        }, "msg");
        if (confirmed)
        {
            RedMineIssues.Clear();
            _dbContext.RedMineIssues.RemoveRange(_dbContext.RedMineIssues);
            Task.Run(async () =>
            {
                await _dbContext.SaveChangesAsync();
                LoadIssues();
            });
        }
    }

    private ICommand? _syncActivitiesCommand;

    public ICommand SyncActivitiesCommand =>
        _syncActivitiesCommand ??= new DelegateCommand(SyncActivitiesFromRedMineServer, CanExecuteSync)
            .ObservesProperty(() => IsSyncing);

    private void SyncActivitiesFromRedMineServer()
    {
        IsSyncing = true;
        Task.Run(() =>
        {
            RedMineUtility.SyncActivities(_dbContext);
            IsSyncing = false;
        });
    }

    private bool CanExecuteSync() => !IsSyncing;

    #endregion


    private RedMineIssue? _selectedRedMineIssue;

    public RedMineIssue? SelectedRedMineIssue
    {
        get => _selectedRedMineIssue;
        set => SetProperty(ref _selectedRedMineIssue, value);
    }

    #region DataLoad

    private void LoadActivities()
    {
        var fetchedData = _dbContext.RedMineActivities.Select(activity => activity).ToList();
        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            RedMineActivities.Clear();
            RedMineActivities.AddRange(fetchedData);
        }));
    }

    private void LoadIssues()
    {
        var fetchedData = _dbContext.RedMineIssues.Select(issue => issue).ToList();
        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            RedMineIssues.Clear();
            RedMineIssues.AddRange(fetchedData);
        }));
    }

    #endregion
}