using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private GameObject _entrance;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

        if(_player != null && _entrance != null)
        {
            _player.transform.position = _entrance.transform.position;
        }
        else
        {
            Debug.LogError("Player or entrance object is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
