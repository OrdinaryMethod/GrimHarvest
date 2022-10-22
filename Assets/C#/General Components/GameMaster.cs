using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public int maxHordeSpawn;
    public bool hordeActive;
    public float setSpawnCooldown;

    void Start()
    {
        hordeActive = false;
    }
}
