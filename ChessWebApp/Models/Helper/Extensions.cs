using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessWebApp.Models.Helper
{
    public static class Extensions
    {
        public static HtmlString EnumToString<T>(this IHtmlHelper helper)
        {
            var values = Enum.GetValues(typeof(T)).Cast<int>();
            var enumDictionary = values.ToDictionary(key => key, value => Enum.GetName(typeof(T), value));

            return new HtmlString(JsonConvert.SerializeObject(enumDictionary));
        }
    }
}
