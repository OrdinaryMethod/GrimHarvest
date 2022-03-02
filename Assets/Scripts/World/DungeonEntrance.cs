using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    [SerializeField] private DungeonMasterData _dungeonMasterData;

    [SerializeField] private string _dungeonName;

    [SerializeField] private List<string> _dungeonScenes;

    private bool _selectScene;

    void Start()
    {
        _selectScene = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_dungeonMasterData.dungeonStarted)
        {    
            _dungeonMasterData.dungeonStarted = true;
            _dungeonMasterData.dungeonName = _dungeonName;
            _dungeonMasterData.maxLevel = _dungeonScenes.Count;
            Debug.Log("Dungeon data collected.");

            for (int i = 0; i < _dungeonScenes.Count; i++)
            {
                _dungeonMasterData.dungeonScenes.Add(_dungeonScenes[i]);
                Debug.Log("Collecting Scenes...");
            }
            Debug.Log("Collecting Scenes Complete.");
            _selectScene = true;
        }
        else
        {
            Debug.Log("Grabbing next dungeon scene...");
            _selectScene = true;
        }
    }

    void Update()
    {
        if(_selectScene)
        {
            _selectScene = false; //Reset
            SceneSelect();
        }
    }

    private void SceneSelect()
    {
        int _sceneSelect = Random.Range(0, _dungeonScenes.Count); 
        string _selectedScene = _dungeonScenes[_sceneSelect];

        if (_dungeonMasterData.completedScenes == null || !_dungeonMasterData.completedScenes.Contains(_selectedScene) && _dungeonMasterData.currentLevel < _dungeonMasterData.maxLevel)
        {
            Debug.Log("Loading scene...");
            _dungeonMasterData.currentLevel++;
            _dungeonMasterData.completedScenes.Add(_selectedScene);
            SceneManager.LoadScene(_dungeonScenes[_sceneSelect]);         
        }
        else if (_dungeonMasterData.completedScenes.Contains(_selectedScene) && _dungeonMasterData.currentLevel < _dungeonMasterData.maxLevel)
        {
            Debug.Log("Chosen scene already completed - selecting another scene.");
            _sceneSelect = Random.Range(0, _dungeonScenes.Count);
            _selectedScene = _dungeonScenes[_sceneSelect];
            _selectScene = true;
        }
        else if(_dungeonMasterData.currentLevel == _dungeonMasterData.maxLevel && _dungeonMasterData.maxLevel > 0)
        {
            Debug.Log("Dungeon Completed! Loading overworld and resetting dungeon master data.");

            //Reset dungeon master data before exit
            _dungeonMasterData.dungeonStarted = false;
            _dungeonMasterData.dungeonName = null;
            _dungeonMasterData.currentLevel = 0;
            _dungeonMasterData.maxLevel = 0;
            _dungeonMasterData.dungeonScenes.Clear();
            _dungeonMasterData.completedScenes.Clear();

            SceneManager.LoadScene("Template_Catacomb");
        }
         
    }
}
