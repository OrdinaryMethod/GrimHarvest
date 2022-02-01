#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ScriptableObjManager : EditorWindow
{
    VectorValue vectorValue;
    ObtainedUpgrades obtainedUpgrades;

    [MenuItem("Tools/Manage Scriptable Objects")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ScriptableObjManager));
    }

    private void OnGUI()
    {
        //GUILayout.Label("Spawn Settings", EditorStyles.boldLabel);
        //objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);
        //objectID = EditorGUILayout.IntField("Object ID", objectID);
        //objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.5f, 3f);
        //spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);

        //GUILayout.Label("Objects to Spawn", EditorStyles.boldLabel);
        //hidingSpotPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/HidingSpots/HidingSpot.prefab", typeof(GameObject));
        //barrierPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/Barriers/Barrier.prefab", typeof(GameObject));
        //hazardPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/World/Hazards/Hazard.prefab", typeof(GameObject));

        if (GUILayout.Button("Save Player Position"))
        {
            //SpawnHidingSpot();
        }

        if (GUILayout.Button("Save Obtained Upgrades"))
        {
            //SpawnHidingSpot();
        }
    }
}
#endif
