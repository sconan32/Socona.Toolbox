using Socona.ToolBox.Parametrization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Socona.Expor.Utilities.Options.Parameters
{

    public class TypeListParameter<C> : ListParameter<Type>
    {
        ///<summary>
        /// Class loader
        ///</summary>

        //protected static  ClassLoader loader = ClassLoader.getSystemClassLoader();

        ///<summary>
        /// The restriction class for the list of class names.
        ///</summary>

        protected Type restrictionClass;

        ///<summary>
        /// Constructs a class list parameter with the given optionID and restriction
        /// class.
        /// 
        ///   </summary>
        ///<param name="optionID">the unique id of this parameter</param>
        ///<param name="restrictionClass">the restriction class of the list of class names</param>
        ///<param name="optional">specifies if this parameter is an optional parameter</param>

        public TypeListParameter(OptionAttribute optionID, Type restrictionClass, bool isRequired,) :
            base(optionID, optional)
        {
            this.restrictionClass = restrictionClass;
        }

        ///<summary>
        /// Constructs a class list parameter with the given optionID and restriction
        /// class.
        /// 
        ///  </summary>
        ///<param name="optionID">the unique id of this parameter</param>
        ///<param name="restrictionClass">the restriction class of the list of class names</param>

        public TypeListParameter(UserParameter optionID, Type restrictionClass) :
            base(optionID)
        {
            this.restrictionClass = restrictionClass;
        }

        ///<summary> @inheritDoc </summary>


        public override String GetValueAsString()
        {
            StringBuilder buf = new StringBuilder();
            String defPackage = restrictionClass.Namespace + ".";

            if (GetListSize() > 0)
            {
                foreach (var c in GetValue())
                {
                    if (buf.Length > 0)
                    {
                        buf.Append(LIST_SEP);
                    }
                    String name = c.FullName;
                    // if (name.StartsWith(defPackage))
                    //  {
                    //     name = name.Substring(defPackage.Length);
                    //  }
                    buf.Append(name);
                }
            }
            return buf.ToString();
        }

        ///<summary> @inheritDoc </summary>



        protected override IList<Type> ParseValue(Object obj)
        {
            try
            {
                List<Type> l = (List<Type>)obj;
                // do extra validation:
                //foreach (Type o in l)
                //{
                //    if (!(o is Type))
                //    {
                //        throw new WrongParameterValueException(
                //            "Wrong parameter format for parameter \"" + GetName() + "\". Given list contains objects of different type!");
                //    }
                //}
                // TODO: can we use reflection to get extra checks?
                // TODO: Should we copy the list?
                return (List<Type>)l;
            }
            catch (InvalidCastException)
            {
                // continue with others
            }
            // Did we get a single class?
            try
            {
                if (restrictionClass.IsAssignableFrom((Type)obj))
                {
                    List<Type> clss = new List<Type>(1);
                    clss.Add((Type)obj);
                    return clss;
                }
            }
            catch (InvalidCastException)
            {
                // continue with others
            }
            if (obj is String)
            {
                string objstr = (String)obj;
                if (string.IsNullOrWhiteSpace(objstr))
                {
                    return new List<Type>();
                }
                String[] classes = SPLIT.Split(objstr);
                // TODO: allow empty lists (and list constraints) to enforce Length?
                if (classes.Length == 0)
                {
                    throw new UnspecifiedParameterException(
                        "Wrong parameter format! Given list of classes for parameter \"" +
                        GetName() + "\" is either empty or has the wrong format!");
                }

                List<Type> cls = new List<Type>(classes.Length);
                foreach (String cl in classes)
                {
                    try
                    {
                        Type c;
                        try
                        {
                            c = Type.GetType(cl, throwOnError: true);
                        }

                        catch (TypeLoadException)
                        {
                            // try in package of restriction class
                            c = Type.GetType(restrictionClass.Namespace + "." + cl);
                        }
                        if (c == null)
                        {
                            // try in package of restriction class
                            c = Type.GetType(restrictionClass.Namespace + "." + cl);
                        }
                        // Redundant check, also in validate(), but not expensive.
                        if (!restrictionClass.IsAssignableFrom(c))
                        {
                            throw new WrongParameterValueException(this, cl, "Class \"" + cl + "\" does not extend/implement restriction class " + restrictionClass + ".\n");
                        }
                        else
                        {
                            cls.Add((Type)c);
                        }
                    }
                    catch (TypeLoadException e)
                    {
                        throw new WrongParameterValueException(this, cl, "Class \"" + cl + "\" not found.\n", e);
                    }
                }
                return cls;
            }
            // INCOMPLETE
            throw new WrongParameterValueException(
                "Wrong parameter format! Parameter \"" + GetName() + "\" requires a list of Class values!"
                );
        }

        ///<summary> @inheritDoc </summary>


        protected bool Validate(List<Type> obj)
        {
            List<Type> list = obj as List<Type>;
            foreach (Type cls in list)
            {
                if (!restrictionClass.IsAssignableFrom(cls))
                {
                    throw new WrongParameterValueException(
                        this, cls.Name,
                        "Class \"" + cls.Name + "\" does not extend/implement restriction class " + restrictionClass + ".\n"
                        );
                }
            }
            return base.Validate(obj);
        }

        ///<summary>
        /// Returns the restriction class of this class parameter.
        /// 
        /// </summary>
        ///<returns>the restriction class of this class parameter.</returns>
        public Type GetRestrictionClass()
        {
            return restrictionClass;
        }

        ///<summary>
        /// RawAt an iterator over all known implementations of the class restriction.
        /// 
        /// </summary>
        ///<returns>List object</returns>
        public IList<Type> GetKnownImplementations()
        {
            return InspectionUtil.CachedFindAllImplementations(GetRestrictionClass());
        }

        ///<summary>
        /// Returns a string representation of the parameter's type.
        /// 
        /// </summary>
        ///<returns>&quot;&lt;class_1,...,class_n&gt;&quot;</returns>

        public override String GetSyntax()
        {
            return "<class_1,...,class_n>";
        }

        ///<summary>
        /// Returns a list of new instances for the value (i.e., the class name) of
        /// this class list parameter. The instances have the type of the restriction
        /// class of this class list parameter.
        /// <br/>
        /// If the Class for the class names is not found, the instantiation is tried
        /// using the package of the restriction class as package of the class name.
        /// 
        ///  </summary>
        ///<param name="config">Parameterization to use (if Parameterizable))</param>
        ///<returns>a list of new instances for the value of this class list parameter</returns>
        public virtual IList<C> InstantiateClasses(IParameterization config)
        {
            config = config.Descend(this);
            IList<C> instances = new List<C>();
            if (GetValue() == null)
            {
                config.ReportError(new UnusedParameterException("Value of parameter " + GetName() + " has not been specified."));
                return instances; // empty list.
            }

            foreach (Type cls in GetValue())
            {
                // NOTE: There is a duplication of this code in ObjectListParameter - keep
                // in sync!
                try
                {
                    C instance = (C)ClassGenericsUtil.TryInstantiate(cls, config);
                    instances.Add(instance);
                }
                catch (Exception e)
                {
                    config.ReportError(new WrongParameterValueException(this, cls.Name, e));
                }
            }
            return instances;
        }

        ///<summary>
        /// Provides a description string listing all classes for the given superclass
        /// or interface as specified in the properties.
        /// 
        /// </summary>
        ///<returns>a description string listing all classes for the given superclass
        ///         or interface as specified in the properties</returns>
        public String RestrictionString()
        {
            String prefix = restrictionClass.Namespace + ".";
            StringBuilder info = new StringBuilder();
            if (restrictionClass.IsInterface)
            {
                info.Append("Implementing ");
            }
            else
            {
                info.Append("Extending ");
            }
            info.Append(restrictionClass.Name);
            info.Append(FormatUtil.NEWLINE);

            IList<Type> known = GetKnownImplementations();
            if (known.Count > 0)
            {
                info.Append("Known classes (default package " + prefix + "):");
                info.Append(FormatUtil.NEWLINE);
                foreach (Type c in known)
                {
                    info.Append("->" + FormatUtil.NONBREAKING_SPACE);
                    String name = c.Name;
                    if (name.StartsWith(prefix))
                    {
                        info.Append(name.Substring(prefix.Length));
                    }
                    else
                    {
                        info.Append(name);
                    }
                    info.Append(FormatUtil.NEWLINE);
                }
            }
            return info.ToString();
        }

        ///<summary>
        /// This class sometimes provides a list of value descriptions.
        /// 
        /// <see cref = "de.lmu.ifi.dbs.elki.utilities.optionhandling.parameters.Parameter#hasValuesDescription()"/>
        ///</summary>


        public override bool HasValuesDescription()
        {
            return restrictionClass != null && restrictionClass != typeof(object);
        }

        ///<summary>
        /// Return a description of known valid classes.
        /// 
        /// <see cref = "de.lmu.ifi.dbs.elki.utilities.optionhandling.parameters.Parameter#getValuesDescription()"/>
        ///</summary>


        public override String GetValuesDescription()
        {
            if (restrictionClass != null && restrictionClass != typeof(object))
            {
                return RestrictionString();
            }
            return "";
        }
    }
}
