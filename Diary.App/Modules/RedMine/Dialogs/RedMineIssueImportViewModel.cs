using Diary.App.Database;
using Diary.App.Utilities;
using Diary.Core.Entities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Diary.Core.Base;
using Microsoft.EntityFrameworkCore;

namespace Diary.App.Modules.RedMine.Dialogs;

public class RedMineIssueImportViewModel : BindableBase, IDialogAware
{
    private readonly IAppDbContext _dbContext;

    public bool CanCloseDialog() => true;

    private bool _finishedImport = false;

    public void OnDialogClosed()
    {
        var result = new DialogResult(_finishedImport ? ButtonResult.Yes : ButtonResult.No);
        RequestClose?.Invoke(result);
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }

    public RedMineIssueImportViewModel(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
        QueryResults.CollectionChanged += (sender, args) => { RaisePropertyChanged(nameof(QueryResults)); };
    }

    public string Title { get; } = "导入RedMine问题";
    private int _totalCount;

    public event Action<IDialogResult>? RequestClose;

    #region Properties

    private ObservableCollection<RedMineUtility.DisplayIssue> _queryResults = new ObservableCollection<RedMineUtility.DisplayIssue>();

    public ObservableCollection<RedMineUtility.DisplayIssue> QueryResults
    {
        get => _queryResults;
        private set => SetProperty(ref _queryResults, value);
    }

    private string _searchPattern = "";

    public string SearchPattern
    {
        get => _searchPattern;
        set => SetProperty(ref _searchPattern, value);
    }

    private bool _filterAssignToMe = false;

    public bool FilterAssignToMe
    {
        get => _filterAssignToMe;
        set => SetProperty(ref _filterAssignToMe, value);
    }

    private int _page;

    public int Page
    {
        get => _page;
        set => SetProperty(ref _page, value);
    }

    private int _selectedIssueIndex;

    public int SelectedIssueIndex
    {
        get => _selectedIssueIndex;
        set => SetProperty(ref _selectedIssueIndex, value);
    }

    #endregion Properties

    #region Commands

    private ICommand? _queryIssuesCommand;
    public ICommand QueryIssuesCommand => _queryIssuesCommand ??= new DelegateCommand(ExecuteQueryIssues);

    private void ExecuteQueryIssues()
    {
        Page = 0;
        _totalCount = 0;
        UpdateQuery(UpdateTotalCount);
    }

    private ICommand? _importAllIssuesCommand;

    public ICommand? ImportAllIssuesCommand =>
        _importAllIssuesCommand ??= new DelegateCommand(ExecuteImportAllIssuesCommand, CanExecuteImportAllIssuesCommand)
            .ObservesProperty(() => QueryResults);

    private bool CanExecuteImportAllIssuesCommand() => QueryResults.Count > 0;

    private void ExecuteImportAllIssuesCommand()
    {
        foreach (var issue in QueryResults)
        {
            try
            {
                _dbContext.RedMineIssues.AddAsync(
                    new RedMineIssue()
                    {
                        IssueId = issue.IssueId,
                        IssueName = $"#{issue.IssueId} - {issue.IssueName}（{issue.ProjectName}）"
                    }
                );
                _finishedImport = true;
            }
            catch (Exception)
            {
                // duplicated, do nothing
            }

            (_dbContext as DbContext)!.SaveChangesAsync();
        }
    }

    private ICommand? _importSelectedIssueCommand;

    public ICommand? ImportSelectedIssueCommand =>
        _importSelectedIssueCommand ??= new DelegateCommand(ExecuteImportSelectedIssueCommand, CanExecuteImportSelectedIssueCommand);

    private void ExecuteImportSelectedIssueCommand()
    {
        var issue = QueryResults[SelectedIssueIndex];
        try
        {
            _dbContext.RedMineIssues.AddAsync(
                new RedMineIssue()
                {
                    IssueId = issue.IssueId,
                    IssueName = $"#{issue.IssueId} - {issue.IssueName}（{issue.ProjectName}）"
                }
            );
            _finishedImport = true;
            (_dbContext as DbContext)!.SaveChangesAsync();
        }
        catch (Exception)
        {
            // duplicated, do nothing
        }
    }

    private bool CanExecuteImportSelectedIssueCommand() => SelectedIssueIndex >= 0;

    private ICommand? _previousPageCommand;

    public ICommand? PreviousPageCommand =>
        _previousPageCommand ??= new DelegateCommand(ExecutePreviousPageCommand, CanExecutePreviousPageCommand).ObservesProperty(() => Page);

    private void ExecutePreviousPageCommand()
    {
        Page = Page - 1;
        UpdateQuery(null);
    }

    private bool CanExecutePreviousPageCommand() => Page > 0;

    private ICommand? _nextPageCommand;

    public ICommand? NextPageCommand =>
        _nextPageCommand ??= new DelegateCommand(ExecuteNextPageCommand, CanExecuteNextPageCommand).ObservesProperty(() => Page);

    private void ExecuteNextPageCommand()
    {
        Page = Page + 1;
        UpdateQuery(null);
    }

    private bool CanExecuteNextPageCommand() => Page >= 0 && Page < _totalCount / 10;

    #endregion Commands

    private void UpdateResultList(IEnumerable<RedMineUtility.DisplayIssue> data)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            QueryResults.Clear();
            QueryResults.AddRange(data);
        });
    }

    private void UpdateQuery(Action<int>? action)
    {
        Task.Run(async () =>
        {
            var list = await RedMineUtility
                .QueryIssues(_searchPattern, _filterAssignToMe, _page, action);
            UpdateResultList(list);
        });
    }

    private void UpdateTotalCount(int total)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _totalCount = total;
            RaisePropertyChanged(nameof(Page));
        });
    }
}