using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SpatialAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        // Calculate distance between object and player
        float distance = Vector2.Distance(transform.position, _player.transform.position);

        // Adjust volume based on distance
        _audioSource.volume = 0.5f - (distance / 200);

        // Adjust stereo pan based on position relative to player
        if (transform.position.x > _player.transform.position.x)
        {
            _audioSource.panStereo = (transform.position.x - _player.transform.position.x) / 10;
        }
        else
        {
            _audioSource.panStereo = (_player.transform.position.x - transform.position.x) / -10;
        }
    }
}
