using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMasterData
{
    //Zone spawn location
    public float xCord;
    public float yCord;



    public GameMasterData(GameMaster GM)
    {
        xCord = GM.xCord;
        yCord = GM.yCord;
    }

}
