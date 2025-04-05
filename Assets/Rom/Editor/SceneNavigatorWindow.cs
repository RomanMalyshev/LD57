using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SceneNavigatorWindow : EditorWindow
{
    private List<string> myScenePaths = new List<string>();
    private List<string> otherScenePaths = new List<string>();
    private int selectedTab = 0;
    private Vector2 scrollPosition;
    private static readonly string[] tabNames = { "My Scenes", "Other Scenes" };
    private const string MyScenesPathPrefix = "Assets/AstronomySim/"; // Define the path prefix for your scenes

    [MenuItem("Tools/Scene Navigator")]
    public static void ShowWindow()
    {
        GetWindow<SceneNavigatorWindow>("Scene Navigator");
    }

    private void OnEnable()
    {
        RefreshSceneList();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Project Scenes", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh"))
        {
            RefreshSceneList();
        }
        GUILayout.FlexibleSpace(); // Pushes the toolbar to the right if desired, or remove for left alignment
        selectedTab = GUILayout.Toolbar(selectedTab, tabNames, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        List<string> scenesToDisplay = selectedTab == 0 ? myScenePaths : otherScenePaths;
        string noScenesMessage = selectedTab == 0
            ? $"No scenes found in '{MyScenesPathPrefix}'. Click 'Refresh' to search again."
            : "No other scenes found in the project. Click 'Refresh' to search again.";

        if (scenesToDisplay.Count == 0)
        {
            EditorGUILayout.HelpBox(noScenesMessage, MessageType.Info);
        }
        else
        {
            foreach (string scenePath in scenesToDisplay)
            {
                EditorGUILayout.BeginHorizontal();
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                // Optionally display the path as well for clarity, especially in 'Other Scenes'
                // EditorGUILayout.LabelField($"{sceneName} ({Path.GetDirectoryName(scenePath)})");
                EditorGUILayout.LabelField(sceneName);


                if (GUILayout.Button("Load", GUILayout.Width(60)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void RefreshSceneList()
    {
        myScenePaths.Clear();
        otherScenePaths.Clear();
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets" });

        foreach (string guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            // Use OrdinalIgnoreCase for case-insensitive comparison, robust across OSes
            if (path.StartsWith(MyScenesPathPrefix, System.StringComparison.OrdinalIgnoreCase))
            {
                 myScenePaths.Add(path);
            }
            else
            {
                otherScenePaths.Add(path);
            }
        }

        // Sort both lists alphabetically by name
        myScenePaths = myScenePaths.OrderBy(path => Path.GetFileNameWithoutExtension(path)).ToList();
        otherScenePaths = otherScenePaths.OrderBy(path => Path.GetFileNameWithoutExtension(path)).ToList();

        Repaint(); // Redraw the window after refreshing
    }
} 