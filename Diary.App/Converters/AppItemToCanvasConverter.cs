using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Diary.Core.Base;
using MahApps.Metro.IconPacks;

namespace Diary.App.Converters;

public class AppItemToCanvasConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppMenuItem item)
        {
            if (item.Icon == null)
            {
                // Default Icon
                Canvas canvas = new Canvas();
                canvas.Children.Add(new PackIconModern(){ Kind = PackIconModernKind.App});
                return canvas;
            }
            else
            {
                return item.Icon;
            }
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}