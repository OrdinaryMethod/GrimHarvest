using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    //Variables
    public string sceneName;

    //Zone Spawn Coordinates
    public float xCord;
    public float yCord;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            SceneManager.LoadSceneAsync(sceneName);
            GameObject GM = GameObject.Find("GameMaster");
            if(GM != null)
            {
                GM.GetComponent<GameMaster>().xCord = xCord;
                GM.GetComponent<GameMaster>().yCord = yCord;
                SaveSystem.SaveGameMaster(GM.GetComponent<GameMaster>());
            }
            
        }
    }
}
