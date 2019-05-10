using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Socona.ToolBox.Windows.Parametrization
{
    public interface IBindableParameter : IParameter
    {

        DisplayStyleEnum DisplayStyle { get; }

        ObservableCollection<string> ValidationErrors { get; }
    }

    public enum DisplayStyleEnum
    {
        Text = 0,
        Bool = 1,
        DropDown = 2,
        DropDownText = 3,
        List = 4,
        DateTime = 10,
        Date = 11,
        Time = 12,
        Header=99,
    }
}


