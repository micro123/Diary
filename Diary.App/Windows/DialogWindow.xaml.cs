using Prism.Services.Dialogs;

namespace Diary.App.Windows
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : IDialogWindow
    {
        private IDialogResult? _result = null;

        public DialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result
        {
            get => _result ??= new DialogResult(ButtonResult.Cancel);
            set
            {
                _result = value;
            }
        }
    }
}