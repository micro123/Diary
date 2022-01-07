using Diary.App.Database;
using Diary.App.Events;
using Diary.Core.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Diary.App.ViewModels
{
    internal class ShellWindowViewModel : BindableBase
    {
        private readonly IApplication _application;
        private readonly DiaryDbContext _context;
        private readonly IDialogService _dialogService;

        private string _title = "Diary Center";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public ObservableCollection<AppMenuItem> Menus => _application.Menus;

        private AppMenuItem _selectedItem;

        public AppMenuItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                value.Command?.Execute(null);
            }
        }

        private bool _appPanelIsOpen = true;

        public bool AppPanelIsOpen
        {
            get => _appPanelIsOpen;
            set => SetProperty(ref _appPanelIsOpen, value);
        }

        private ICommand? _toggleAppPanelOpenCommand;

        public ICommand? ToggleAppPanelOpenCommand =>
            _toggleAppPanelOpenCommand ??= new DelegateCommand(ExecuteToggleAppPanelOpenCommand);

        private void ExecuteToggleAppPanelOpenCommand()
        {
            AppPanelIsOpen = !AppPanelIsOpen;
        }

        public ShellWindowViewModel(IApplication application,
            DiaryDbContext context,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
        {
            _application = application;
            _context = context;
            _dialogService = dialogService;
            eventAggregator.GetEvent<AppStartedEvent>().Subscribe(InitViewState);
        }

        private ICommand? _showSettingsCommand;
        public ICommand ShowSettingsCommand => _showSettingsCommand ??= new DelegateCommand(ShowSettingsDialog);

        private void ShowSettingsDialog()
        {
            _dialogService.ShowDialog("SettingsDialog");
        }

        public void InitViewState()
        {
            SelectedItem = Menus.First();
        }
    }
}