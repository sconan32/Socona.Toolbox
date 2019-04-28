using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socona.Expor.Utilities.Options.Parameters
{

    ///<summary>
    /// Parameter class for a parameter specifying a list of vectors.
    ///
    /// @author Steffi Wanka
    /// @author Erich Schubert
    ///</summary>


    public class VectorListParameter : ListParameter<double[]>
    {
        ///<summary>
        /// Constructs a vector list parameter with the given name and description.
        ///
        ///   </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="constraint">Constraint</param>
        ///<param name="defaultValue">Default value</param>

        public VectorListParameter(UserParameter optionID, IParameterConstraint constraint, IList<double[]> defaultValue)
            :
                base(optionID, defaultValue)
        {
            AddConstraint(constraint);
        }

        ///<summary>
        /// Constructs a vector list parameter with the given name and description.
        ///
        ///   </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="constraint">Constraint</param>
        ///<param name="optional">Optional flag</param>

        public VectorListParameter(UserParameter optionID, IParameterConstraint constraint, bool optional)
            : base(optionID, optional)
        {
            AddConstraint(constraint);
        }

        ///<summary>
        /// Constructs a vector list parameter with the given name and description.
        ///
        ///  </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="constraint">Constraint</param>

        public VectorListParameter(UserParameter optionID, IParameterConstraint constraint)
            : base(optionID)
        {
            AddConstraint(constraint);
        }

        ///<summary>
        /// Constructs a vector list parameter with the given name and description.
        ///
        ///  </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="defaultValue">Default value</param>
        // Indiscernible from optionID, constraints
        /*
     * public VectorListParameter(OptionID optionID, List<Vector> defaultValue) {
     * super(optionID, defaultValue); }
     */

        ///<summary>
        /// Constructs a vector list parameter with the given name and description.
        ///
        ///  </summary>
        ///<param name="optionID">Option ID</param>
        ///<param name="optional">Optional flag</param>

        public VectorListParameter(UserParameter optionID, bool optional)
            : base(optionID, optional)
        {
        }

        ///<summary>
        /// Constructs a vector list parameter with the given name and description.
        ///
        /// </summary>
        ///<param name="optionID">Option ID</param>

        public VectorListParameter(UserParameter optionID)
            : base(optionID)
        {
        }


        public override String GetValueAsString()
        {
            StringBuilder buf = new StringBuilder();
            IList<double[]> val = GetValue();
            // Iterator<Vector> valiter = val.iterator();
            for (int i = 0; i < val.Count; i++)
            {
                buf.Append(FormatUtil.Format(val[i], LIST_SEP));
                // Append separation character
                if (i == val.Count - 1)
                {
                    buf.Append(VECTOR_SEP);
                }
            }
            return buf.ToString();
        }


        protected override IList<double[]> ParseValue(Object obj)
        {
            List<double[]> vecs = new List<double[]>();
            try
            {
                var type = obj.GetType();
                if (obj is IEnumerable)
                {
                    foreach (var o in (IEnumerable)obj)
                    {
                        var list = new List<double>();

                        if (o is IEnumerable)
                        {
                            foreach (var o1 in (IEnumerable)o)
                            {
                                if (!(o1 is Double || o1 is int || o1 is float || o1 is long))
                                {
                                    throw new WrongParameterValueException("Wrong parameter format for parameter \"" +
                                                                           GetName() +
                                                                           "\". Given list contains objects of different type!");
                                }
                                else
                                {
                                    list.Add((double)o1);
                                }
                            }

                        }
                        vecs.Add(list.ToArray());
                    }
                }
            }
            catch (Exception )
            {
                // continue with other attempts.
            }
            if (obj is String)
            {
                String[] vectors = VECTOR_SPLIT.Split((String)obj);
                if (vectors.Length == 0)
                {
                    throw new WrongParameterValueException(
                        "Wrong parameter format! Given list of vectors for parameter \"" + GetName() + "\" is empty!");
                }
                // ArrayList<Vector> vecs = new ArrayList<>();

                List<double> vectorCoord = new List<double>();
                foreach (String vector in vectors)
                {
                    vectorCoord.Clear();
                    String[] coordinates = SPLIT.Split(vector);
                    foreach (String coordinate in coordinates)
                    {
                        try
                        {
                            vectorCoord.Add(Double.Parse(coordinate));
                        }
                        catch (FormatException )
                        {
                            throw new WrongParameterValueException("Wrong parameter format! Coordinates of vector \"" +
                                                                   vector + "\" are not valid!");
                        }
                    }
                    vecs.Add(vectorCoord.ToArray());
                }
                return vecs;
            }
            throw new WrongParameterValueException("Wrong parameter format! Parameter \"" + GetName() +
                                                   "\" requires a list of double values!");
        }


        public override int GetListSize()
        {
            if (GetValue() == null)
            {
                return 0;
            }

            return GetValue().Count;
        }

        ///<summary>
        /// Returns a string representation of the parameter's type.
        ///
        /// </summary>
        ///<returns>&quot;&lt;double_11,...,double_1n:...:double_m1,...,double_mn&gt;&
        ///         quot ;</returns>

        public override String GetSyntax()
        {
            return "<double_11,...,double_1n:...:double_m1,...,double_mn>";
        }
    }

}
