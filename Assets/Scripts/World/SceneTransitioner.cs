using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    //Variables
    public string sceneName;
    public Vector2 playerPos;
    public VectorValue playerMemory;
   

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collision.isTrigger)
        {
            playerMemory.initialValue = playerPos;
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
