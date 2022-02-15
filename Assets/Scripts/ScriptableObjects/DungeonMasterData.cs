using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonMasterData : ScriptableObject
{
    [Header("Dungeons")]
    public bool dungeonStarted;
    public string dungeonName;
    public int currentLevel;
    public int maxLevel;

    [Header("Dungeons")]
    public List<string> dungeonScenes;


}
