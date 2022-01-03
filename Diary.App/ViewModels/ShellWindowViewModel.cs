using Diary.Core.Base;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Diary.App.Database;
using Prism.Commands;
using Prism.Services.Dialogs;

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

        public ShellWindowViewModel(IApplication application, DiaryDbContext context, IDialogService dialogService)
        {
            _application = application;
            _context = context;
            _dialogService = dialogService;
        }

        private ICommand? _showSettingsCommand;
        public ICommand ShowSettingsCommand => _showSettingsCommand ??= new DelegateCommand(ShowSettingsDialog);

        private void ShowSettingsDialog()
        {
            _dialogService.ShowDialog("SettingsDialog");
        }
    }
}
