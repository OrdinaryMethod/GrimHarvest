using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    //Variables
    public Transform LandingPos;

    public Transform climpPoint1;
    public Transform climpPoint2;
    public Transform climpPoint3;

    [SerializeField] private bool isRightLedge;

    void Start()
    {
        gameObject.transform.localScale = new Vector3(20f, 3.5f, 3.5f);

        if(isRightLedge)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        
    }
}
