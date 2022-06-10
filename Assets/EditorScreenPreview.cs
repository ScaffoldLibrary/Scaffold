using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Assets.Scaffold.Launcher.Editor
{
    public class EditorScreenPreview : MonoBehaviour
    {

        [MenuItem("Assets/Editor Skins")]
        static public void SaveEditorSkin()
        {
            GUISkin skin = ScriptableObject.Instantiate(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)) as GUISkin;
            AssetDatabase.CreateAsset(skin, "Assets/EditorSkin.guiskin");
        }
    }
}