using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class ScriptableObjectWindow : EditorWindow
{
    private List<string> _directories;
    private int _previousIndex;
    private Vector2 _scrollPos;
    private string[] _guids = new string[0];
    private int _selectedIndex = -1;
    private GUIStyle _customLabelStyle = new GUIStyle(EditorStyles.label);

    [MenuItem("Window/Scriptable Object Organizer")]
    static void Init()
    {
        ScriptableObjectWindow window = (ScriptableObjectWindow)EditorWindow.GetWindow(typeof(ScriptableObjectWindow));
        window.Show();
    }


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        // Left side of the window
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        DisplayScriptableObjectsList();
        EditorGUILayout.EndVertical();
        
        
        // Right side of the window
        EditorGUILayout.BeginVertical();
        DisplayOpenObjectFolder();
        DisplayScriptableObjectInspector();
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    
    private void DisplayScriptableObjectInspector()
    {
        if (_selectedIndex >= 0 && _selectedIndex < _guids.Length)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(_guids[_selectedIndex]);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            _customLabelStyle.fontSize = 20;
            GUILayout.Label(obj.name, _customLabelStyle, GUILayout.Height(40));

            
            Editor scriptableObjectEditor = Editor.CreateEditor(obj);
            scriptableObjectEditor.OnInspectorGUI();
        }
    }

    private string _selectedDirectory = "Assets";

    private void DisplayOpenObjectFolder()
    {
        if (GUILayout.Button("Open Object"))
        {
            int index = 0;
            foreach (string guid in _guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                bool selected = index == _selectedIndex;
                selected = EditorGUILayout.ToggleLeft(obj.name, selected);
                if (selected)
                {
                    _selectedIndex = index;
                    EditorGUIUtility.PingObject(obj);
                }
                index++;
            }
        }    }
    private void DisplayScriptableObjectsList()
    {
        _customLabelStyle.fontSize = 15;
        GUILayout.Label("Select a folder:", _customLabelStyle);
        if (GUILayout.Button("Open Folder"))
        {
            _selectedDirectory = EditorUtility.OpenFolderPanel("Select a folder", _selectedDirectory, "");
            _selectedDirectory = _selectedDirectory.Replace(Application.dataPath, "Assets");
        }
        GUILayout.Label(Path.GetFileName(_selectedDirectory), _customLabelStyle);

        if (GUILayout.Button("Load Scriptable Objects"))
        {
            _customLabelStyle.fontSize = 13;
            _guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { _selectedDirectory });
            Debug.Log(_guids.Length);
            foreach (string guid in _guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                Debug.Log(obj);
            }
        }

        GUILayout.Label("SO List", _customLabelStyle);
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        int index = 0;
        foreach (string guid in _guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            bool selected = index == _selectedIndex;
            selected = EditorGUILayout.ToggleLeft(obj.name, selected);
            if (selected)
            {
                _selectedIndex = index;
                
            }
            index++;
        }
        EditorGUILayout.EndScrollView();
    }
}