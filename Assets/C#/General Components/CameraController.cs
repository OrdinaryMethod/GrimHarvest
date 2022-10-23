using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;

    void Update()
    {
        if(_player == null)
        {
            _player = GameObject.Find("Player").transform;
        }
        else
        {
            Vector3 desiredPos = _player.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, _speed * Time.deltaTime);
        }   
    }
}
