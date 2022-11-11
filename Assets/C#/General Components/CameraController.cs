using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;

    public bool isMinimap;

    void Update()
    {
        if(_player == null)
        {
            _player = GameObject.Find("Player").transform;
        }
        else
        {
            if(!isMinimap)
            {
                if (!_player.gameObject.GetComponent<PlayerMonitor>().playerIsDead)
                {
                    Vector3 desiredPos = _player.position + _offset;
                    transform.position = Vector3.Lerp(transform.position, desiredPos, _speed * Time.deltaTime);
                }
            }
            else
            {
                if(!_player.gameObject.GetComponent<PlayerMonitor>().playerIsDead)
                {
                    Vector3 desiredPos = _player.position;
                    transform.position = new Vector3(desiredPos.x, desiredPos.y, -100);
                }              
            }                        
        }   
    }
}
