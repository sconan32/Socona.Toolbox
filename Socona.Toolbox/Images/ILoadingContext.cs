using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Images
{
    public interface ILoadingContext<TResult> where TResult : class
    {
        object Current { get; set; }

        double? DesiredHeight { get; }

        double? DesiredWidth { get; }

        byte[] HttpResponseBytes { get; set; }

        object OriginSource { get; }

        TResult Result { get; set; }
    }
}
