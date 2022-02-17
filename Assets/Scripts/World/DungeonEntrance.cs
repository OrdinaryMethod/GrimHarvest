using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    [SerializeField] private DungeonMasterData dungeonMasterData;

    [SerializeField] private string _dungeonName;

    [SerializeField] private List<string> _dungeonScenes;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!dungeonMasterData.dungeonStarted)
        {
            dungeonMasterData.dungeonStarted = true;
            dungeonMasterData.dungeonName = _dungeonName;
            dungeonMasterData.currentLevel = 1;
            dungeonMasterData.maxLevel = _dungeonScenes.Count;

            for (int i = 0; i < _dungeonScenes.Count; i++)
            {
                dungeonMasterData.dungeonScenes.Add(_dungeonScenes[i]);
            }

            //Scene select
            int sceneSelect = Random.Range(0, _dungeonScenes.Count);
            SceneManager.LoadScene(_dungeonScenes[sceneSelect]);
        }
        else
        {
            //Choose a level based off of the data stored in the dungeon master object
            int sceneSelect = Random.Range(0, _dungeonScenes.Count);
            SceneManager.LoadScene(_dungeonScenes[sceneSelect]);
        }
        


    }
}
