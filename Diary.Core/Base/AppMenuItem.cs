using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diary.Core.Base;

public struct AppMenuItem
{
    public string Title { get; set; } = string.Empty;

    public ICommand? Command { get; set; } = null;

    public Canvas? Icon { get; set; }

    public static Canvas CreateCanvasFromPackIcon<T>(T icon) where T : UIElement
    {
        Canvas canvas = new Canvas();
        canvas.Children.Add(icon);
        return canvas;
    }
}