using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ScriptableObjectsEditor : EditorWindow
{
    private GUIStyle _customLabelStyle = new GUIStyle(EditorStyles.label);
    private Vector2 _scrollPos;
    private List<Type> _scriptableObjectTypes = new List<Type>();
    private HashSet<string> _objectsGuids = new HashSet<string>();
    private SerializedObject _serializedObject;
    private List<string> _selectedGuids = new List<string>();

    private int _selectedIndex = -1;
    int _selectedObjectTypeIndex = 0;
    private int _lastIndexSelected = 0;
    private bool _isOpenedFirstTime = false;

    private const string WINDOW_TITLE = "Scriptable Objects Organizer";
    private const string OBJECTS_FILTER = "t:ScriptableObject";
    private const string INITIAL_FOLDER = "Assets";
    private string _selectedDirectory = INITIAL_FOLDER;
    private string _currentOpenDirectory;


    [MenuItem("Window/Scriptable Object Organizer")]
    private static void Init()
    {
        ScriptableObjectsEditor window = (ScriptableObjectsEditor)EditorWindow.GetWindow(typeof(ScriptableObjectsEditor));
        window.Show();
    }

    private void OnGUI()
    {
        _customLabelStyle.fontSize = 25;
        _customLabelStyle.alignment = TextAnchor.UpperCenter;
        GUILayout.Label(WINDOW_TITLE, _customLabelStyle);

        //Left Side
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(300));
        DisplaySelectedFolder();
        EditorGUILayout.EndVertical();
       
        // Right Side
        EditorGUILayout.BeginVertical();
        DisplaySelectedScriptableObject();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DisplaySelectedScriptableObject()
    {
        if (_selectedIndex >= 0 && _selectedIndex < _selectedGuids.Count)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(_selectedGuids[_selectedIndex]);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

            _customLabelStyle.fontSize = 20;
            GUILayout.Label(obj.name, _customLabelStyle, GUILayout.Height(40));
            DisplayOpenObjectFolder();

            EditorGUI.BeginChangeCheck();
            _serializedObject = new SerializedObject(obj);
            _serializedObject.Update();
            Editor scriptableObjectEditor = Editor.CreateEditor(_serializedObject.targetObject);
            scriptableObjectEditor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                _serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(obj);
            }
        }
    }

    private void DisplaySelectedFolder()
    {
        _customLabelStyle.fontSize = 15;
        GUILayout.Label("Select a folder:", _customLabelStyle);
        if (GUILayout.Button("Open Folder", GUILayout.Height(30)))
        {
            _selectedDirectory = EditorUtility.OpenFolderPanel("Select a folder", _selectedDirectory, "");
            _selectedDirectory = _selectedDirectory.Replace(Application.dataPath, INITIAL_FOLDER);
            GUILayout.Label(Path.GetFileName(_selectedDirectory), _customLabelStyle);
            _isOpenedFirstTime = true;
        }

        LoadScriptableObjectsInFolder();
    }

    private void LoadScriptableObjectsInFolder()
    {
        if (CheckFolderAvailibity())
        {
            _currentOpenDirectory = _selectedDirectory;
            _customLabelStyle.fontSize = 13;
            _objectsGuids = new HashSet<string>(AssetDatabase.FindAssets(OBJECTS_FILTER, new[] { _selectedDirectory }));
            _lastIndexSelected = -1;
            _selectedObjectTypeIndex = 0;
            GetScriptableObjectTypes();
        }

        string[] objectNames = new string[_scriptableObjectTypes.Count];
        int indexType = 0;
        foreach (Type objectType in _scriptableObjectTypes)
        {
            objectNames[indexType] = objectType.ToString();
            indexType++;
        }
        _selectedObjectTypeIndex = EditorGUILayout.Popup("SO Types", _selectedObjectTypeIndex, objectNames);
        SetCurrentTypeSelected();
        
        _customLabelStyle.fontSize = 13;
        GUILayout.Label(" List", _customLabelStyle);
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        
        DisplayScriptableObjectsInList();
        EditorGUILayout.EndScrollView();
    }

    private void DisplayScriptableObjectsInList()
    {
        int index = 0;
        foreach (string guid in _selectedGuids)
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
    }

    private bool CheckFolderAvailibity()
    {
        bool isAvailable = !string.IsNullOrEmpty(_selectedDirectory) && _currentOpenDirectory != _selectedDirectory
            && _isOpenedFirstTime;
        return isAvailable;
    }

    private void DisplayOpenObjectFolder()
    {
        if (GUILayout.Button("Open Object", GUILayout.Height(30)))
        {
            bool selected = _selectedIndex != -1;
            if (!selected)
                return;

            string assetPath = AssetDatabase.GUIDToAssetPath(_selectedGuids[_selectedIndex]);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            EditorGUIUtility.PingObject(obj);
        }
    }

    private void GetScriptableObjectTypes()
    {
        _scriptableObjectTypes.Clear();
        foreach (string guid in _objectsGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            
            if (_scriptableObjectTypes.Contains(asset.GetType()))
                continue;

            _scriptableObjectTypes.Add(asset.GetType());
        }
    }

    private void SetCurrentTypeSelected()
    {
        if (_lastIndexSelected == _selectedObjectTypeIndex)
            return;

        _lastIndexSelected = _selectedObjectTypeIndex;
        _selectedGuids.Clear();
        foreach (string guid in _objectsGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            Type currentType = _scriptableObjectTypes[_selectedObjectTypeIndex];
            if (asset.GetType().ToString() == currentType.ToString())
            {
                _selectedGuids.Add(guid);
            }
        }
    }

}

