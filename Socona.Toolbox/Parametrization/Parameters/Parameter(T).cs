using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Socona.ToolBox.Parametrization.Parameters
{
    /// <summary>
    ///     算法中的参数
    /// </summary>
    /// <typeparam name="T">参数的值类型</typeparam>
    public class Parameter<T> : IParameter        
    {
        protected readonly List<ValidationAttribute> m_constraints;

        protected readonly OptionAttribute m_option;

        protected T m_defaultValue = default;

        protected object m_givenValue = default;
        /// <summary>
        /// Identify a parameter if it is a necessary one. it can take a default value
        /// </summary>
        protected bool m_isRequired = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool m_isOptional = false;

        protected string m_description;

        protected bool m_isDefaultValue;

        /// <summary>
        /// For Value type
        /// </summary>
        protected T m_value;

        protected List<string> m_validationErrors = new List<string>();
        public IEnumerable<string> ValidateErrors => m_validationErrors;
        ///<summary>
        /// Constructs a parameter with the given optionID, constraints, and default
        /// value.
        ///   </summary>
        ///<param name="optionID">the unique id of this parameter</param>
        ///<param name="constraints">the constraints of this parameter, may be empty if there
        ///        are no constraints</param>
        ///<param name="defaultValue">the default value of this parameter (may be null)</param>
        public Parameter(OptionAttribute optionID, bool isRequired = false, T defaultValue = default, IEnumerable<T> candidates = null, IEnumerable<ValidationAttribute> constraints = null)
        {
            m_option = optionID;
            m_description = optionID.Description;

            this.Name = optionID.Name;
            this.FullName = optionID.FullName;
            this.m_isRequired = isRequired;
            this.m_defaultValue = defaultValue;
            HasDefaultValue = true;
            if (candidates != null)
            {
                Candidates.AddRange(candidates);
            }
            this.m_isOptional = HasDefaultValue;
            this.m_constraints = new List<ValidationAttribute>();
            if (constraints != null)
            {
                this.m_constraints.AddRange(constraints);
            }
        }

        public T DefaultValue
        {
            get { return m_defaultValue; }
            set
            {
                m_defaultValue = value;
                m_isOptional = true;
                HasDefaultValue = true;
            }
        }

        public virtual string GetDefaultValueAsString()
        {
            return m_defaultValue?.ToString();
        }

        ///<summary>
        /// Checks if this parameter has a default value.
        /// </summary>
        ///<returns>true, if this parameter has a default value, false otherwise</returns>
        public virtual bool HasDefaultValue { get; private set; }

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
            // Assume default value instead.
            if (HasDefaultValue)
            {
                Value = m_defaultValue;
                GivenValue = m_defaultValue;
                m_isDefaultValue = true;
                return true;
            }
            if (IsOptional)
            {
                // Optional is fine, but not successful
                return false;
            }
            throw new ParameterException($"{Name} is a required parameter, default value is not valid");
        }

        object IParameter.DefaultValue { get => m_defaultValue; set => SetDefaultValue(value); }

        protected void SetDefaultValue(object value)
        {
            if (TryParse(value, out T valT))
            {
                DefaultValue = valT;
            }
            else
            {
                throw new InvalidParameterValueException("");
            }

        }

        ///<summary>
        /// Checks if this parameter is an optional parameter.
        /// 
        /// </summary>
        ///<returns>true if this parameter is optional, false otherwise</returns>
        public bool IsOptional => m_isOptional;


        ///<summary>
        /// Whether this class has a list of default values.
        /// </summary>
        ///<returns>whether the class has a description of valid values.</returns>

        public virtual bool HasCandidates => Candidates.Any();

        public List<T> Candidates { get; } = new List<T>();

        private string _candidatesString;

        public string GetCandidatesAsString(string splitString = "|")
        {
            if (_candidatesString == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var cand in Candidates)
                {
                    sb.Append(cand.ToString()).Append(splitString);
                }
                sb.Remove(sb.Length - splitString.Length, sb.Length);
                _candidatesString = sb.ToString();
            }
            return _candidatesString;
        }

        ///<summary>
        /// Get the UserParameter of this option.        
        /// </summary>
        public OptionAttribute Option => m_option;

        ///<summary>
        /// Get the name of the option.
        /// </summary>
        public string Name { get; private set; }

        public string FullName { get; set; }

        ///<summary>
        /// Get the last given value. May return <code>null</code>        
        /// </summary>
        public virtual object GivenValue
        {
            get => m_givenValue;
            set { m_givenValue = value; TrySetValue(value); }
        }


        ///<summary>
        /// Checks if the given argument is valid for this option.
        ///   </summary>
        ///<param name="obj">option value to be checked</param>
        ///<returns>true, if the given value is valid for this option</returns>
        ///<exception cref="ParameterException">if the given value is not a valid value for this
        ///         option.
        ///</exception>
        public bool ValidateValue(Object obj, out T @value)
        {
            return TryParse(obj, out @value) && ValidateConstraints(@value);
        }

        ///<summary>
        /// Add an additional constraint.
        /// </summary>
        ///<param name="constraint">Constraint to add.</param>
        public void AddConstraint(ValidationAttribute constraint)
        {
            m_constraints.Add(constraint);
        }

        public virtual T Value
        {
            get { return m_value; }
            set
            {
                this.m_value = value;
                m_givenValue = value;
                IsValid = true;
            }
        }

        ///<summary>
        /// Sets the value of the option. 
        /// </summary>
        ///<param name="obj">the option's value to be set
        ///</param>
        public virtual void SetValue(Object obj)
        {
            T val = Parse(obj);
            if (ValidateConstraints(val))
            {
                Value = val;
            }
            else
            {
                IsValid = false;
                throw new InvalidParameterValueException("Value for option \"" + Name + "\" did not validate: " + obj);
            }
        }

        public virtual bool TrySetValue(object obj)
        {
            m_validationErrors.Clear();
            if (TryParse(obj, out T valT) && ValidateConstraints(valT, false))
            {
                Value = valT;
                return true;
            }
            m_validationErrors.Add("给定的值不合法");
            IsValid = false;
            return false;
        }

        public virtual bool IsValid { get; protected set; }

        ///<summary>
        /// Get the value as string. May return <code>null</code>
        /// 
        /// </summary>
        ///<returns>Value as string</returns>
        public virtual string GetValueAsString()
        {
            return m_value?.ToString();
        }

        object IParameter.Value { get => m_value; set => SetValue(value); }

        IEnumerable IParameter.Candidates => Candidates;

        protected virtual bool ValidateConstraints(T obj, bool throwIfFailed = true)
        {

            foreach (var cons in m_constraints)
            {
                var result = cons.GetValidationResult(obj, new ValidationContext(this));
                if (result != ValidationResult.Success)
                {
                    m_validationErrors.Add(result.ErrorMessage);
                    if (throwIfFailed)
                    {
                        throw new InvalidParameterValueException("Specified parameter value for parameter \"" + Name +
                                                               "\" breaches parameter constraint.\n" + result.ErrorMessage);
                    }
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if ((obj as Parameter<T>)?.m_option.Equals(this.m_option) == true)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return m_option.GetHashCode();
        }

        protected virtual T Parse(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("Parameter Error.\n" + "No value for parameter \"" + Name + "\" " + "given.");
            }
            if (TryParse(obj, out T value))
            {
                return value;
            }
            throw new InvalidParameterValueException(this, obj.ToString(), "Given Value is not valid");
        }

        protected virtual bool TryParse(object obj, out T value)
        {
            if (obj is T valueT)
            {
                value = valueT;
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        ///     get or set the short description of the option
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public IReadOnlyCollection<ValidationAttribute> Constraints => m_constraints;

        public bool IsDefaultValue => m_isDefaultValue;

        public bool IsEmpty => m_isDefaultValue || IsValid || !(Value.Equals(default(T)));

        public bool IsRequired => m_isRequired;

        public override string ToString()
        {
            return $"{m_option.Name} : {this.GetType().Name} = {Value?.ToString() ?? "<null>"}({Description})";
        }

        public virtual string PlaceHolder => $"<parameter of {typeof(T).Name}>";

        ///<summary>
        /// Returns the extended description of the option which includes the option's
        /// type, the short description and the default value (if specified).
        /// </summary>
        public virtual string GetFullDescription()
        {
            var description = new StringBuilder();
            // description.Append(getParameterType()).Append(" ");
            description.Append(description);
            description.Append(Environment.NewLine);
            if (HasCandidates)
            {
                var valuesDescription = GetCandidatesAsString();
                description.Append(valuesDescription);
                if (!valuesDescription.EndsWith(Environment.NewLine))
                {
                    description.Append(Environment.NewLine);
                }
            }
            if (HasDefaultValue)
            {
                description.Append("Default: ");
                description.Append(GetDefaultValueAsString());
                description.Append(Environment.NewLine);
            }
            if (m_constraints.Count > 0)
            {
                if (m_constraints.Count == 1)
                {
                    description.Append("Constraint: ");
                }
                else if (m_constraints.Count > 1)
                {
                    description.Append("Constraints: ");
                }
                for (int i = 0; i < m_constraints.Count; i++)
                {
                    var constraint = m_constraints[i];
                    if (i > 0)
                    {
                        description.Append(", ");
                    }
                    description.Append(constraint.ToString());
                    if (i == m_constraints.Count - 1)
                    {
                        description.Append(".");
                    }
                }
                description.Append(Environment.NewLine);
            }
            return description.ToString();
        }

    }

}