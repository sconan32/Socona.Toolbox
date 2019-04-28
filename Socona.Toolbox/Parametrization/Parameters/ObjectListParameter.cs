using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Socona.Expor.Utilities.Options.Parameterizations;

namespace Socona.Expor.Utilities.Options.Parameters
{
    public class ObjectListParameter<C> : TypeListParameter<C>
    {
        ///<summary>
        /// Cache for the generated instances.
        ///</summary>

        private List<C> instances = null;

        ///<summary>
        /// Constructor with optional flag.
        /// 
        ///   </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="restrictionClass">Restriction class</param>
        ///<param name="optional">optional flag</param>
        public ObjectListParameter(UserParameter optionID, Type restrictionClass, bool optional) :
            base(optionID, restrictionClass, optional)
        {
        }

        ///<summary>
        /// Constructor for non-optional.
        /// 
        ///  </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="restrictionClass">Restriction class</param>
        public ObjectListParameter(UserParameter optionID, Type restrictionClass) :
            base(optionID, restrictionClass)
        {
        }

        ///<summary> @inheritDoc </summary>


        public override String GetSyntax()
        {
            return "<object_1|class_1,...,object_n|class_n>";
        }

        ///<summary> @inheritDoc </summary>



        protected override IList<Type> ParseValue(Object obj)
        {
            if (obj == null)
            {
                throw new UnspecifiedParameterException("Parameter Error.\n" + "No value for parameter \"" + this.GetName() + "\" " + "given.");
            }
            if (obj is IList<C>)
            {
                IList<C> l = (IList<C>)obj;
                List<C> inst = new List<C>(l.Count);
                List<Type> classes = new List<Type>(l.Count);
                foreach (Object o in l)
                {
                    // does the given objects class fit?
                    if (restrictionClass.IsInstanceOfType(o))
                    {
                        inst.Add((C)o);
                        classes.Add((Type)o.GetType());
                    }
                    else
                    {
                        if (restrictionClass.IsAssignableFrom((Type)o))
                        {
                            inst.Add(default(C));
                            classes.Add((Type)o);
                        }
                        else
                        {
                            throw new WrongParameterValueException(this, ((Type)o).Name, "Given class not a subclass / implementation of " + restrictionClass.Name, null);
                        }
                    }

                }
                this.instances = inst;
                return base.ParseValue(classes);
            }
            // Did we get a single instance?
            try
            {
                C inst = (C)obj;
                this.instances = new List<C>(1);
                this.instances.Add(inst);
                return base.ParseValue(inst.GetType());
            }
            catch (Exception)
            {
                // Continue
            }
            return base.ParseValue(obj);
        }

       

        public override IList<C> InstantiateClasses(IParameterization config)
        {
            if (instances == null)
            {
                // instantiateClasses will descend itself.
                instances = new List<C>(base.InstantiateClasses(config));
            }
            else
            {
                IParameterization cfg = null;
                for (int i = 0; i < instances.Count; i++)
                {
                    if (instances[(i)] == null)
                    {
                        Type cls = GetValue()[i];
                        try
                        {
                            // Descend at most once, and only when needed
                            if (cfg == null)
                            {
                                cfg = config.Descend(this);
                            }
                            C instance = (C)ClassGenericsUtil.TryInstantiate(cls, cfg);
                            instances[i] = instance;
                        }
                        catch (Exception e)
                        {
                            config.ReportError(new WrongParameterValueException(this, cls.Name, e));
                        }
                    }
                }
            }
            return new List<C>(instances);
        }
    }
}
