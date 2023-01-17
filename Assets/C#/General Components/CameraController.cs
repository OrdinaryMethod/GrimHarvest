using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _playerAimDir;
    private float _xOffset;

    public bool isMinimap;

    public float zoomSpeed;
    public float zoomMin;
    public float zoomMax;
    public bool isZooming;
    private float desiredZoom;

    void Start()
    {
        if(_player == null)
        {
            _player = GameObject.Find("Player").GetComponent<Transform>();
        }

        transform.position = new Vector3(_player.position.x, _player.position.y, 0);
    }
      
    void Update()
    {
        if(_player.gameObject.GetComponent<PlayerController>().facingRight)
        {
            _xOffset = _offset.x;
        }
        else
        {
            _xOffset = -_offset.x;
        }

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
                    _playerAimDir = GameObject.Find("PlayerCameraAimDir").transform;
                    GameObject player = GameObject.Find("Player");

                    if (player != null && _playerAimDir != null)
                    {
                        if (player.GetComponent<PlayerController>().isAiming)
                        {
                            Vector3 desiredPos = _playerAimDir.position + new Vector3(_xOffset, 0, _offset.z);
                            transform.position = Vector3.Lerp(transform.position, desiredPos, _speed * Time.deltaTime);
                        }
                        else
                        {
                            Vector3 desiredPos = _player.position + new Vector3(_xOffset, 0, _offset.z);
                            transform.position = Vector3.Lerp(transform.position, desiredPos, _speed * Time.deltaTime);
                        }
                    }
                    
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

        CameraZoom();
    }

    private void CameraZoom()
    {
        if(!isMinimap)
        {
  
            if(_player.gameObject.GetComponent<PlayerController>().isAiming && _player.gameObject.GetComponent<PlayerController>().isCrouching)
            {
                desiredZoom = Mathf.Lerp(Camera.main.orthographicSize, zoomMax, zoomSpeed * Time.deltaTime);
                Camera.main.orthographicSize = desiredZoom;
            }
            else //Reset
            {
                desiredZoom = Mathf.Lerp(Camera.main.orthographicSize, zoomMin, zoomSpeed * Time.deltaTime);
                Camera.main.orthographicSize = desiredZoom;
            }
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomMin, zoomMax);
        }
    }
}
