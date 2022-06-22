using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.Editor
{
    public class Popup
    {
        public static bool Assert(string title, string text, string yesOption, string noOption)
        {
            return EditorUtility.DisplayDialog(title, text, yesOption, noOption);
        }
    }
}