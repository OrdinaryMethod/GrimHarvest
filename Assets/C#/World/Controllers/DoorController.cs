using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] _switches;
    private Animator _anim;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();     
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOpen)
        {
            int totalSwitchesOn = 0;

            foreach (GameObject s in _switches)
            {
                if (s.GetComponent<Switch>().switchOn)
                {
                    totalSwitchesOn++;
                }
            }

            if (totalSwitchesOn == _switches.Length)
            {
                isOpen = true;
                _anim.SetBool("IsOpen", true);
            }
        }        
    }
}
