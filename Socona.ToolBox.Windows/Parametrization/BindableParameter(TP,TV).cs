using Socona.ToolBox.Parametrization;
using Socona.ToolBox.Parametrization.Parameters;
using Socona.ToolBox.Windows.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Windows.Parametrization
{
    public class BindableParameter<TP, TV> : BindableBase, IBindableParameter
        where TP : Parameter<TV>
    {
        TP _parameter;

        ///<summary>
        /// Constructs a parameter with the given optionID, constraints, and default
        /// value.
        ///   </summary>
        ///<param name="optionID">the unique id of this parameter</param>
        ///<param name="constraints">the constraints of this parameter, may be empty if there
        ///        are no constraints</param>
        ///<param name="defaultValue">the default value of this parameter (may be null)</param>
        public BindableParameter(TP parameter)
        {
            _parameter = parameter;

        }

        public TV DefaultValue
        {
            get { return _parameter.DefaultValue; }
            set
            {
                _parameter.DefaultValue = value;
                RaisePropertyChanged();
            }
        }



        ///<summary>
        /// Checks if this parameter has a default value.
        /// </summary>
        ///<returns>true, if this parameter has a default value, false otherwise</returns>
        public virtual bool HasDefaultValue => _parameter.HasDefaultValue;


        ///<summary>
        /// Handle default values for a parameter.
        ///  </summary>
        ///<returns>Return code: <code>true</code> if it has a default value, <code>false</code>
        ///         if it is optional without a default value. Exception if it is a
        ///         required parameter!</returns>
        ///<exception cref="UnspecifiedParameterException">If the parameter requires a value
        ///</exception>
        public bool UseDefaultValue()
        {
            var result = _parameter.UseDefaultValue();
            RaisePropertyChanged(nameof(GivenValue));
            RaisePropertyChanged(nameof(Value));
            return result;
        }

        object IParameter.DefaultValue
        {
            get => _parameter.DefaultValue;
            set => ((IParameter)_parameter).DefaultValue = value;
        }



        ///<summary>
        /// Checks if this parameter is an optional parameter.
        /// 
        /// </summary>
        ///<returns>true if this parameter is optional, false otherwise</returns>
        public bool IsOptional => _parameter.IsOptional;


        ///<summary>
        /// Whether this class has a list of default values.
        /// </summary>
        ///<returns>whether the class has a description of valid values.</returns>

        public virtual bool HasCandidates => _parameter.HasCandidates;

        public List<TV> Candidates => _parameter.Candidates;


        ///<summary>
        /// Get the UserParameter of this option.        
        /// </summary>
        public OptionAttribute Option => _parameter.Option;

        ///<summary>
        /// Get the name of the option.
        /// </summary>
        public string Name => _parameter.Name;

        public string FullName { get => _parameter.FullName; set => _parameter.FullName = value; }

        ///<summary>
        /// Get the last given value. May return <code>null</code>        
        /// </summary>
        public object GivenValue
        {
            get
            {
                if (_parameter.IsValid)
                {
                    return _parameter.GivenValue;
                }
                else
                {
                    _parameter.UseDefaultValue();
                    return _parameter.GivenValue;
                }

            }
            set
            {
                _parameter.GivenValue = value;
                RaisePropertyChanged();
                if (_parameter.TrySetValue(value))
                {
                    RaisePropertyChanged(nameof(Value));
                }
                ValidationErrors.Clear();
                foreach(var err in  _parameter.ValidateErrors)
                {
                    ValidationErrors.Add(err);
                }
            }
        }



        ///<summary>
        /// Add an additional constraint.
        /// </summary>
        ///<param name="constraint">Constraint to add.</param>
        public void AddConstraint(ValidationAttribute constraint)
        {
            _parameter.AddConstraint(constraint);
        }

        public TV Value
        {
            get { return _parameter.Value; }
            set
            {
                _parameter.Value = value;
                RaisePropertyChanged();
            }
        }


        public virtual bool IsValid { get => _parameter.IsValid; }


        object IParameter.Value
        {
            get => _parameter.Value;
            set
            {
                _parameter.SetValue(value);
                RaisePropertyChanged();
            }
        }

        IEnumerable IParameter.Candidates => _parameter.Candidates;



        public override bool Equals(object obj)
        {
            if ((obj as BindableParameter<TP, TV>)?._parameter.Equals(this._parameter) == true)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _parameter.GetHashCode();
        }



        /// <summary>
        ///     get or set the short description of the option
        /// </summary>
        public string Description
        {
            get { return _parameter.Description; }
            set { _parameter.Description = value; RaisePropertyChanged(); }
        }

        public IReadOnlyCollection<ValidationAttribute> Constraints => _parameter.Constraints;

        public bool IsDefaultValue => _parameter.IsDefaultValue;

        public bool IsEmpty => _parameter.IsEmpty;

        public bool IsRequired => _parameter.IsRequired;

        public string PlaceHolder => _parameter.PlaceHolder;

        public DisplayStyleEnum DisplayStyle
        {
            get
            {
                if (typeof(TV) == typeof(bool)) return DisplayStyleEnum.Bool;
                if (typeof(TV) == typeof(DateTime)) return DisplayStyleEnum.Time;
                if (typeof(TV) == typeof(Type)) return DisplayStyleEnum.Header;
                return DisplayStyleEnum.Text;
            }
        }

        public ObservableCollection<string> ValidationErrors => new ObservableCollection<string>();

        public override string ToString()
        {
            return $"Bindable Proxy of Parameter [{_parameter.ToString()}]";
        }

        public string GetFullDescription()
        {
            return _parameter.GetFullDescription();
        }
    }
}
