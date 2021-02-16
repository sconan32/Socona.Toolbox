using System;
using System.Collections.Generic;
using System.Text;

namespace Socona.ToolBox.Windows.ViewModel
{
    public class ViewModelBase : BindableBase
    {
        public ViewModelBase()
        { }

        protected bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set { Set(ref _isLoading, value); RaisePropertyChanged(nameof(IsReady)); }
        }
        public bool IsReady => !_isLoading;

        public event EventHandler<string> StatusTextChanged;

        protected string _statusText;

        public string StatusText
        {
            get => _statusText;
            set
            {
                if(value != _statusText)
                {
                    Set(ref _statusText, value);
                    StatusTextChanged?.Invoke(this, value);
                }
            }
        }

        public bool IsDebugging
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

    }
}