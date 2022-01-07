using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace Diary.App.Windows
{
    /// <summary>
    /// MessageDialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageDialogWindow : MetroWindow, IDialogWindow
    {
        public MessageDialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }
}