using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace Diary.App.Windows
{
    /// <summary>
    /// MessageDialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageDialogWindow: MetroWindow, IDialogWindow
    {
        public MessageDialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }
}
