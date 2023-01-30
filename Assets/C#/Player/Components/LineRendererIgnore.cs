using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererIgnore : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("test tst");
        if (other.tag == "Switch")
        {
            
            // Skip triggering events for specific colliders
            return;
        }

        // Add code for handling other trigger colliders
    }
}
