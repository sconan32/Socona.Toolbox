using Microsoft.Collections.Extensions;
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
        List<IParameter> _paramters = new List<IParameter>();


        IParameter _root;
        ISettings _rootInstance;

        Dictionary<IParameter, IParameter> _parentMap = new Dictionary<IParameter, IParameter>();


        MultiValueDictionary<IParameter, IParameter> _childMap = new MultiValueDictionary<IParameter, IParameter>();

        public List<IParameter> Parameters => _paramters;
        public ParametrizationContext(ISettings settingModel)
        {
            this._rootInstance = settingModel;
            if (ParameterFactory.Instance.TryBuildFromType(settingModel.GetType(), out _root))
            {
                CascadeBuildInternal(_rootInstance, _root);
            }
        }
        public void CascadeBuildInternal(ISettings settingModel, IParameter parentParameter)
        {
            var properties = settingModel.GetType().GetProperties();
            foreach (var prop in properties)
            {
                //处理显示声明的参数
                if (typeof(IParameter).IsAssignableFrom(prop.GetType()))
                {
                    var para = (IParameter)prop.GetValue(settingModel);
                    AddParameter(para, parentParameter);
                }
                //处理通过Attribute声明的参数
                else if (ParameterFactory.Instance.TryBuildFromProperty(prop, out IParameter parameter))
                {
                    AddParameter(parameter, parentParameter);
                    if(parameter is TypeParameter)
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
            if (!_paramters.Contains(parameter))
            {
                _paramters.Add(parameter);
                _parentMap[parameter] = parent;
                _childMap.Add(parent, parameter);
            }
        }



    }
}
