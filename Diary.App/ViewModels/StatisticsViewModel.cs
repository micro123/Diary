using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Diary.App.Database;
using Diary.Core.Entities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Diary.App.ViewModels;

public class StatisticsViewModel: BindableBase
{
    private readonly DiaryDbContext _dbContext;
    private readonly IDialogService _dialogService;

    public StatisticsViewModel(DiaryDbContext dbContext, IDialogService dialogService)
    {
        _dbContext = dbContext;
        _dialogService = dialogService;
        _itemCategories.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(ItemCategories));

        LoadItemTypes();
    }

    private ObservableCollection<ItemType> _itemCategories = new();

    public ObservableCollection<ItemType> ItemCategories
    {
        get => _itemCategories;
        set => SetProperty(ref _itemCategories, value);
    }

    private string _newTagName = "";

    public string NewTagName
    {
        get => _newTagName;
        set => SetProperty(ref _newTagName, value);
    }

    private ICommand? _addTagCommand;

    public ICommand? AddTagCommand =>
        _addTagCommand ??= new DelegateCommand(ExecuteAddTagCommand, CanExecuteAddTagCommand)
            .ObservesProperty(() => NewTagName);

    private void ExecuteAddTagCommand()
    {
        var name = NewTagName;
        NewTagName = "";
        Task.Run(() =>
        {
            try
            {
                _dbContext.ItemTypes.Add(new ItemType() {Title = name});
                _dbContext.SaveChanges();
                LoadItemTypes();
            }
            catch (Exception)
            {
                // ignored
            }
        });
    }

    private bool CanExecuteAddTagCommand() => !string.IsNullOrWhiteSpace(NewTagName);

    private ICommand? _removeTagCommand;

    public ICommand? RemoveTagCommand =>
        _removeTagCommand ??= new DelegateCommand<ItemType>(ExecuteRemoveTagCommand);

    private void ExecuteRemoveTagCommand(ItemType itemType)
    {
        Task.Run(() =>
        {
            try
            {
                _dbContext.ItemTypes.Remove(itemType);
                _dbContext.SaveChanges();
                LoadItemTypes();
            }
            catch (Exception)
            {
                // ignored
            }
        });
    }

    private async void LoadItemTypes()
    {
        List<ItemType> result;
        await Task.Run(() =>
        {
            result = _dbContext.ItemTypes.Select(item => item).ToList();
            Application.Current.Dispatcher.Invoke(() =>
            {
                ItemCategories.Clear();
                ItemCategories.AddRange(result);
            });
        });
    }
}