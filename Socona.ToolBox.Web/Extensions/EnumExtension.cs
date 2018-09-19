using Newtonsoft.Json;
using System;

using System.Linq;
using System.Reflection;

namespace Socona.ImVehicle.Infrastructure.Extensions
{
    public static class EnumExtension
    {
        //public static string GetDisplayName<T>(this T enumValue)
        //{
        //    string returnName = null;
        //    try
        //    {
        //        returnName = enumValue.GetType()
        //           .GetMember(enumValue.ToString())
        //           ?.First()
        //           ?.GetCustomAttribute(Type.GetType("DisplayAttribute"))
        //           ?.GetName();
        //    }
        //    catch (Exception)
        //    { }
        //    if (returnName == null)
        //    {
        //        returnName = enumValue?.ToString()??"";
        //    }
        //    return returnName;
        //}

        //public static HtmlString EnumToHtmlString<T>(this IHtmlHelper helper)
        //{
        //    var values = Enum.GetValues(typeof(T)).Cast<T>();
        //    var enumDictionary = values.Select(v => new { name = GetDisplayName(v), value = v, });
        //    return new HtmlString(JsonConvert.SerializeObject(enumDictionary));
        //}

    }
}
