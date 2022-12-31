using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{    
    public VectorValue playerPosition;
    public Vector2 setPlayerPosition;
    [SerializeField] private bool facingRight;

    public string sceneName;
    

    void Start()
    {
        //SceneManager.LoadScene("Template_Catacomb");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerPosition.initialValue = setPlayerPosition;
        playerPosition.facingRight = facingRight;
        SceneManager.LoadScene(sceneName);
    }   
}
