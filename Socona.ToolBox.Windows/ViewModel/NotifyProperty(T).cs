
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Socona.ToolBox.Windows.ViewModel
{
    public interface IRaisePropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
    }


    public class NotifyProperty<T> : INotifyPropertyChanged
    {

        private string _name;

        private T _value;

        private IRaisePropertyChanged _owner;



        public NotifyProperty(IRaisePropertyChanged owner, string name, T initialValue)
        {
            _owner = owner;
            _name = name;
            _value = initialValue;
        }
        public string Name => _name;

        public T Value => _value;

        public void SetValue(T newValue)
        {
            if (newValue.Equals(_value))
            {
                _value = newValue;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        ///     Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">
        ///     Name of the property used to notify listeners.  This
        ///     value is optional and can be provided automatically when invoked from compilers
        ///     that support <see cref="CallerMemberNameAttribute" />.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public static class NotifyPropertyHelper
    {
        public static NotifyProperty<T> CreateNotifyProperty<T>(
            this IRaisePropertyChanged owner,
            string name, T initialValue)
        {
            return new NotifyProperty<T>(owner,
              name,
              initialValue);
        }
    }

}