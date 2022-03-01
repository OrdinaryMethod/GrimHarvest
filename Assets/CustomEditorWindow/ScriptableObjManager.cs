#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjManager : EditorWindow
{
    VectorValue vectorValue;
    ObtainedUpgrades obtainedUpgrades;
    DungeonMasterData dungeonMasterData;

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
        vectorValue = (VectorValue)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Player/PlayerPosition.asset", typeof(VectorValue));
        spawnPos = EditorGUILayout.Vector2Field("Initial Value", spawnPos);
        facingRight = EditorGUILayout.Toggle("Facing Right", facingRight);
    
        if (GUILayout.Button("Save Player Spawn Position"))
        {
            SaveSpawnPosition();
        }

        GUILayout.Label("Obtained Upgrades", EditorStyles.boldLabel);
        obtainedUpgrades = (ObtainedUpgrades)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Player/ObtainedUpgrades.asset", typeof(ObtainedUpgrades));
        wallClimbing = EditorGUILayout.Toggle("Wall Climbing", wallClimbing);

        if (GUILayout.Button("Save Obtained Upgrades"))
        {
            SaveObtainedUpgrades();
        }

        GUILayout.Label("Dungeon Master Data", EditorStyles.boldLabel);
        dungeonMasterData = (DungeonMasterData)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/World/Dungeons/DungeonMaster.asset", typeof(DungeonMasterData));
        
        if (GUILayout.Button("Reset Dungeon Master Data"))
        {
            ResetDungeonMasterData();
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

    private void ResetDungeonMasterData()
    {
        dungeonMasterData.dungeonStarted = false;
        dungeonMasterData.dungeonName = null;
        dungeonMasterData.currentLevel = 0;
        dungeonMasterData.maxLevel = 0;
        dungeonMasterData.dungeonScenes = null;
    }
}
#endif
