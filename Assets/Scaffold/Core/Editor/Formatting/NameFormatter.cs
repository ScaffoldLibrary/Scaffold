using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scaffold.Core.Editor
{
    public static class NameFormatter
    {
        //KEY => com.scaffold.sample
        //DEFINE => USE_SCAFFOLD_SAMPLE
        //PASCAL => Sample

        public static string KeyToDefine(string key)
        {
            key = KeyToPascal(key).ToUpper();
            return $"USE_SCAFFOLD_{key}";
        }

        public static string KeyToPascal(string key)
        {
            Regex regex = new Regex(@"(?<=scaffold.).*?(?=\.|$)");
            key = regex.Match(key).Value;
            key = key.ToLower().Replace(".", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            key = info.ToTitleCase(key).Replace(" ", string.Empty);
            return key;
        }
    }
}