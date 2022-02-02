#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ScriptableObjManager : EditorWindow
{
    VectorValue vectorValue;
    ObtainedUpgrades obtainedUpgrades;

    //Spawn Position
    Vector2 spawnPos;
    bool facingRight;

    //Obtained Upgrades
    bool wallClimbing;


    [MenuItem("Tools/Manage Scriptable Objects")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ScriptableObjManager));
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Spawn Position", EditorStyles.boldLabel);
        vectorValue = (VectorValue)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/PlayerPosition.asset", typeof(VectorValue));
        spawnPos = EditorGUILayout.Vector2Field("Initial Value", spawnPos);
        facingRight = EditorGUILayout.Toggle("Facing Right", facingRight);
    
        if (GUILayout.Button("Save Player Spawn Position"))
        {
            SaveSpawnPosition();
        }
        GUILayout.Label("Obtained Upgrades", EditorStyles.boldLabel);
        obtainedUpgrades = (ObtainedUpgrades)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/ObtainedUpgrades.asset", typeof(ObtainedUpgrades));
        wallClimbing = EditorGUILayout.Toggle("Wall Climbing", wallClimbing);

        if (GUILayout.Button("Save Obtained Upgrades"))
        {
            SaveObtainedUpgrades();
        }
    }

    private void SaveSpawnPosition()
    {
        vectorValue.initialValue = spawnPos;
        vectorValue.facingRight = facingRight;
    }

    private void SaveObtainedUpgrades()
    {
        obtainedUpgrades.wallClimbing = wallClimbing;
    }
}
#endif
