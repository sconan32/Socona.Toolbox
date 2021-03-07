using Microsoft.Collections.Extensions;
//using Newtonsoft.Json;
using Socona.ToolBox.Parametrization.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Socona.ToolBox.Parametrization
{
    public class ParametrizationContext
    {
        readonly IParameter _root;
        readonly ISettings _rootInstance;
        readonly Type _rootInstanceType;
        readonly Dictionary<IParameter, IParameter> _parentMap = new Dictionary<IParameter, IParameter>();
        readonly MultiValueDictionary<IParameter, IParameter> _childMap = new MultiValueDictionary<IParameter, IParameter>();

        public List<IParameter> Parameters { get; } = new List<IParameter>();
        public ParametrizationContext(ISettings settingModel)
        {
            this._rootInstance = settingModel;
            this._rootInstanceType = settingModel.GetType();
            if (ParameterFactory.Instance.TryBuildFromType(settingModel.GetType(), out _root))
            {
                CascadeBuildInternal(_rootInstance, _root);
            }





        }
        public ParametrizationContext(Type settingModelType)
        {
            this._rootInstanceType = settingModelType;
            //this._rootInstance = settingModel;
            //if (ParameterFactory.Instance.TryBuildFromType(settingModel.GetType(), out _root))
            //{
            //    CascadeBuildInternal(_rootInstance, _root);
            //}
        }
        public void CascadeBuildInternal(ISettings settingModel, IParameter parentParameter)
        {
            var properties = settingModel.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var paraValue = prop.GetValue(settingModel);
                //处理显示声明的参数
                if (typeof(IParameter).IsAssignableFrom(prop.PropertyType))
                {
                    AddParameter((IParameter)paraValue, parentParameter);
                }
                //处理通过Attribute声明的参数
                else if (ParameterFactory.Instance.TryBuildFromProperty(prop, out IParameter parameter, paraValue))
                {
                    AddParameter(parameter, parentParameter);
                    if (parameter is TypeParameter)
                    {
                        ISettings settings = (ISettings)prop.GetValue(settingModel);
                        CascadeBuildInternal(settings, parameter);
                    }
                }

            }
        }

        public ParametrizationContext BuildChildren(IParameter parameter)
        {
            //    var parameters = _childMap[parameter];
            //    foreach (var para in parameters)
            //    {
            //        BuildChildren(para);
            //    }
            return this;
        }


        public void AddParameter(IParameter parameter, IParameter parent = null)
        {
            if (parent == null)
            {
                parent = _root;
            }
            if (!Parameters.Contains(parameter))
            {
                Parameters.Add(parameter);
                _parentMap[parameter] = parent;
                _childMap.Add(parent, parameter);
            }
        }

        //public string SerializeToJson()
        //{
        //    return JsonConvert.SerializeObject(_rootInstance);
        //}

        //public ISettings DeserializeFromJson(string text)
        //{
        //    return  (ISettings)JsonConvert.DeserializeObject(text, _rootInstanceType);
        //}
    }
}
