using Diary.Core.Base;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Diary.App.Database;

namespace Diary.App.ViewModels
{
    internal class ShellWindowViewModel : BindableBase
    {
        private readonly IApplication _application;
        private readonly DiaryDbContext _context;

        private string _title = "Diary";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public ObservableCollection<MenuNode> Menus => _application.Menus;

        public ShellWindowViewModel(IApplication application, DiaryDbContext context)
        {
            _application = application;
            _context = context;

            int a = 0;
        }
    }
}
