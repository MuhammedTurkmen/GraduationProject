using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectWindow : EditorWindow
{
    [MenuItem("My Windows/Scriptable Object Creator")]
    public static void ShowWindow()
    {
        ScriptableObjectWindow window = GetWindow<ScriptableObjectWindow>("Scriptables");
        window.Show();
    }

    readonly string path = "Assets/Scripts/Scriptable Objects";

    private readonly float _height = 25;

    private void OnGUI()
    {
        string[] fileNames = Directory.GetFiles(path, "*.cs");

        foreach (var name in fileNames)
        {
            int e = name.LastIndexOf('\\');
            e++;
            string fileName = name.Substring(e, name.Length - e - 3);

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button(fileName, GUILayout.Height(_height)))
            {
                ScriptableObject s = CreateInstance(fileName); 
                //AssetDatabase.CreateAsset(s, "Assets/Objects/Scriptable Objects/" + s.GetType() + ".asset");
                ProjectWindowUtil.CreateAsset(s, "Assets/Objects/Scriptable Objects/Items/" + s.GetType() + ".asset");
            }

            EditorGUILayout.EndVertical();
        }
    }
}