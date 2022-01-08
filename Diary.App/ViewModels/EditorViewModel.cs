using System.Collections.ObjectModel;
using System.Windows.Input;
using Diary.Core.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Diary.App.ViewModels;

public class EditorViewModel: BindableBase, INavigationAware
{
    private readonly IAppDbContext _dbContext;
    private ObservableCollection<object> _diaryTreeItems = new();

    public ObservableCollection<object> DiaryTreeItems
    {
        get => _diaryTreeItems;
        set => SetProperty(ref _diaryTreeItems, value);
    }

    public EditorViewModel(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private ICommand? _jumpToTodayCommand;

    public ICommand? JumpToTodayCommand =>
        _jumpToTodayCommand ??= new DelegateCommand(ExecuteJumpToTodayCommand);

    private void ExecuteJumpToTodayCommand()
    {
        // TODO: Implitement
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        
    }
}