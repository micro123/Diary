using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Diary.App.Dialogs.VM;

public class MessageViewModel: BindableBase, IDialogAware
{
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        PositiveButtonCommand  = new DelegateCommand(PositiveButtonClicked);
        NegativeButtonCommand = new DelegateCommand(NegativeButtonClicked);

        if (parameters.ContainsKey("title"))
        {
            Title = parameters.GetValue<string>("title");
        }

        if (parameters.ContainsKey("desc"))
        {
            Description = parameters.GetValue<string>("desc");
        }

        if (parameters.ContainsKey("p_text"))
        {
            PositiveButtonText = parameters.GetValue<string>("p_text");
        }

        if (parameters.ContainsKey("n_text"))
        {
            NegativeButtonText = parameters.GetValue<string>("n_text");
        }
    }

    private string _description = "";

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _positiveButtonText = "确定";

    public string PositiveButtonText
    {
        get => _positiveButtonText;
        set => SetProperty(ref _positiveButtonText, value);
    }

    private string _negativeButtonText = "取消";

    public string NegativeButtonText
    {
        get => _negativeButtonText;
        set => SetProperty(ref _negativeButtonText, value);
    }

    #region Buttons

    public ICommand? PositiveButtonCommand { get; private set; }
    public ICommand? NegativeButtonCommand { get; private set; }

    private void NegativeButtonClicked()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.No));
    }

    private void PositiveButtonClicked()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Yes));
    }

    #endregion


    public string Title { get; set; } = "提示信息";
    public event Action<IDialogResult>? RequestClose;
}