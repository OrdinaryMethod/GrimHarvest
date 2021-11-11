using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidBomb : MonoBehaviour
{
    [SerializeField] private float bombTimer;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 3);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, bombTimer);
    }
}
