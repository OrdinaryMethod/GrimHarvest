using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroidController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Transform dockPos;
    private PlayerController playerController;
    [SerializeField] private GameObject droidLight;
    public GameObject droidBombPrefab;
    public Transform bombDropPoint;

    private bool droidActive;
    private float moveInputX;
    private float moveInputY;
    [SerializeField] private float speed;


    // Start is called before the first frame update
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        dockPos = GameObject.Find("DroidDock").transform;
        droidActive = false;
        droidLight.SetActive(false); //Light
    }

    // Update is called once per frame
    void Update()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (!droidActive)
        {
            _agent.SetDestination(dockPos.position);
            droidLight.SetActive(false); //Light
        }
        else
        {
            playerController.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, 0);
            droidLight.SetActive(true); //Light
            DropBomb();

            Movement();
        }

        playerController.droidActive = droidActive;

        ActivateDroid();
        
    }

    private void ActivateDroid()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!droidActive)
            {
                if(playerController.isGrounded)
                {
                    droidActive = true;
                }                    
            }
            else
            {
                droidActive = false;
            }      
        }
    }

    private void Movement()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveInputX, moveInputY) * speed;
    }

    private void DropBomb()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(droidBombPrefab, bombDropPoint.transform.position, gameObject.transform.rotation);
        }
    }
}
