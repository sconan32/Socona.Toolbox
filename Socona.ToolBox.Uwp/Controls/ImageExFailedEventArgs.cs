using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.ToolBox.Uwp.Controls
{
    public class ImageExFailedEventArgs : EventArgs
    {
        public ImageExFailedEventArgs(object source, Exception failedException)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Exception = failedException ?? throw new ArgumentNullException(nameof(failedException));
        }
        public Exception Exception { get; }
        public object Source { get; }
    }
}
