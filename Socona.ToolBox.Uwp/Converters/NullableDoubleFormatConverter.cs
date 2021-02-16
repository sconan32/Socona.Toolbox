using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Socona.ToolBox.Uwp.Converters
{
    public class NullableDoubleFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double dval)
            {
                if(parameter is string format)
                {
                    string fullFormat = string.Concat("{0:", format, "}");
                    return string.Format(fullFormat,dval);
                }
                return $"{dval}";
            }
            return string.Empty;
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
