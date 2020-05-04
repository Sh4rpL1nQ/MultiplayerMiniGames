using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MiniGames.Models.Helper
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
