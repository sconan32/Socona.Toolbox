using System;
using System.Collections.Generic;
using System.Text;


namespace Socona.ToolBox.Parametrization
{
    /// <summary>
    /// Models an option specification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class OptionAttribute : Attribute
    {
       // private string helpText;
      //  private string metaValue;
        private readonly string _fullName;
        private readonly string _name;
        private string setName;
        private char separator;

        private string _description;

        private OptionAttribute(string shortName, string longName) : base()
        {
            this._name = shortName ?? throw new ArgumentNullException("shortName");
            this._fullName = longName ?? throw new ArgumentNullException("longName");
            setName = string.Empty;
            separator = '\0';
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.OptionAttribute"/> class.
        /// The default long name will be inferred from target property.
        /// </summary>
        public OptionAttribute()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.OptionAttribute"/> class.
        /// </summary>
        /// <param name="longName">The long name of the option.</param>
        public OptionAttribute(string longName)
            : this(string.Empty, longName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.OptionAttribute"/> class.
        /// </summary>
        /// <param name="shortName">The short name of the option.</param>
        /// <param name="longName">The long name of the option or null if not used.</param>
        public OptionAttribute(char shortName, string longName)
            : this(shortName.ToString(), longName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLine.OptionAttribute"/> class.
        /// </summary>
        /// <param name="shortName">The short name of the option..</param>
        public OptionAttribute(char shortName)
            : this(shortName.ToString(), string.Empty)
        {
        }

        /// <summary>
        /// Gets long name of this command line option. This name is usually a single english word.
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
        }

        /// <summary>
        /// Gets a short name of this command line option, made of one character.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or sets the option's mutually exclusive set name.
        /// </summary>
        public string SetName
        {
            get { return setName; }
            set
            {
                setName = value ?? throw new ArgumentNullException("value");
            }
        }

        /// <summary>
        /// When applying attribute to <see cref="System.Collections.Generic.IEnumerable{T}"/> target properties,
        /// it allows you to split an argument and consume its content as a sequence.
        /// </summary>
        public char Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        public string Description { get => _description; set => _description = value; }
    }
}
