using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Diary.Core.Entities;

namespace Diary.App.Converters;

public class ActivityListToTextConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<RedMineActivity> activities)
        {
            if (!activities.Any())
                return "点击右侧按钮获取服务器上的定义";
            else
            {
                var sb = new StringBuilder();
                foreach (var activity in activities)
                {
                    sb.Append(activity.Name);
                    sb.Append('、');
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
        }

        return "Not Acceptable Value!!";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}