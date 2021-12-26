using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diary.Core;
using Prism.Mvvm;

namespace Diary.App.ViewModels
{
    internal class ShellWindowViewModel: BindableBase
    {
        private readonly IApplication _application;

        private string _title = "Diary";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public ObservableCollection<MenuNode> Menus => _application.Menus;

        public ShellWindowViewModel(IApplication application)
        {
            _application = application;
        }
    }
}
