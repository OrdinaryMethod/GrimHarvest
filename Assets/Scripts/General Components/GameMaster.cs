using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //Variables
    GameObject player;

    //Zone Spawn Location
    public float xCord;
    public float yCord;


    // Update is called once per frame
    void Start()
    {
        
        GameMasterData data = SaveSystem.LoadGameMaster();
        if(data != null)
        {
            xCord = data.xCord;
            yCord = data.yCord;
        }
        

        player = GameObject.Find("Player");
        player.transform.position = new Vector3(xCord,yCord,0);
    }

}
