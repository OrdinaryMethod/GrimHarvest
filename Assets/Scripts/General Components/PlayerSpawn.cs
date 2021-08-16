using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    public bool playerIsDead;

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(player == null)
        {
            Instantiate(playerPrefab, gameObject.transform.position, Quaternion.identity);
        }
    }
}
