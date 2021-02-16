using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Socona.ToolBox.Uwp.Converters
{
    public class NullToBooleanTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
           value == null ? true : false;

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotSupportedException();

    }
}
