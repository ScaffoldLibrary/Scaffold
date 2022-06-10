using UnityEditor;
using UnityEngine;

namespace Scaffold.Launcher.Editor
{
    internal static class LauncherStyles
    {

        public static GUIStyle HeaderBox = new GUIStyle(GUI.skin.window)
        {
            margin = new RectOffset(5, 5, 5, 5),
            alignment = TextAnchor.LowerCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 30,
            stretchWidth = true,
            fixedHeight = 70,
        };

        public static GUIStyle ProjectState = new GUIStyle(GUI.skin.textArea)
        {
            border = new RectOffset(6, 6, 4, 4),
            padding = new RectOffset(2, 2, 1, 3),
            margin = new RectOffset(6, 6, 2, 3),
            alignment = TextAnchor.LowerCenter,
            fontSize = 12,
            clipping = TextClipping.Clip,
        };

        public static GUIStyle ModuleBox = new GUIStyle(GUI.skin.window)
        {
            margin = new RectOffset(5, 5, 5, 5),
            padding = new RectOffset(5, 5, 5, 5),
            stretchHeight = false,
        };

        public static GUIStyle Button = new GUIStyle(GUI.skin.button)
        {
            stretchWidth = false,
        };

        public static GUIStyle CornerButton = new GUIStyle(GUI.skin.button)
        {
            stretchWidth = false,
        };

        public static GUIStyle ModuleDescription = new GUIStyle(GUI.skin.textField)
        {
            stretchWidth = true,
            wordWrap = true,
        };

        public static GUIStyle ModuleName = new GUIStyle()
        {
            stretchWidth = true,
            wordWrap = true,
            padding = new RectOffset(5, 5, 0, 5),
            normal = new GUIStyleState() { textColor = Color.white }
        };

        public static Texture2D ColoredTexture(Color color)
        {
            Color[] pix = new Color[2 * 2];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }
            Texture2D result = new Texture2D(2, 2);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}