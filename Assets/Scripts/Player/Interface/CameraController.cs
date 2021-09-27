using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variables
    [SerializeField] private bool FollowPlayer;

    // Update is called once per frame
    void Update()
    {
        if(FollowPlayer)
        {
            GameObject player = GameObject.Find("Player");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
    }
}
