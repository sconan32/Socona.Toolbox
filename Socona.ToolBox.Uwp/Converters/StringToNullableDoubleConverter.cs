using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Socona.ToolBox.Uwp.Converters
{
    public class StringToNullableDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        { return value; }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double temp;
            if (string.IsNullOrEmpty((string)value) || !double.TryParse((string)value, out temp)) return null;
            else return temp;
        }

    }
}
