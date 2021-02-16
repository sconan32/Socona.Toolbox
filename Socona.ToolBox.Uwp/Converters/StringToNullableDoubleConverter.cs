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
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double? temp = null;
            if (value is string strValue)
            {
                if (double.TryParse(strValue, out double dval))
                {
                    temp = dval;
                }
            }
            return temp;
        }
    }
}
