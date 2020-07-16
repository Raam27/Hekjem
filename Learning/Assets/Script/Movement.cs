using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    
    public int health = 100;
    [SerializeField]
    int rotationSpeed;
    private Rigidbody2D r2d;
    public float runSpeed;
    public float walkSpeed;
    public CircleCollider2D cc2d;
    public Animator anim;
    public AudioManager audim;
    Vector2 direction;
    float horizontalInput;
    float verticalInput;
    float rotateInput;
    Vector3 worldPosition;
    float walkSound;
    float runSound;

    // Start is called before the first frame update
    private void Awake()
    {
        audim = GameObject.FindGameObjectWithTag("audim").GetComponent<AudioManager>() ;
    }
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        cc2d = gameObject.GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        rotatePlayer();


    }
    private void FixedUpdate()
    {
        movePlayer();
        if (horizontalInput == 0 && verticalInput == 0)
        {
            cc2d.radius = 0f;
        }
    }
    private void movePlayer()
    {
      
        if (horizontalInput !=0 || verticalInput != 0)
        {
            if (runSound == 0 && r2d.velocity.magnitude>2.9 )
            {
                audim.Play("Run");
                audim.Stop("Walk");
                audim.Play("BreathRun");
            }
            else if (walkSound == 0  &&r2d.velocity.magnitude < 2.9 )
            {
                audim.Play("Walk");
                audim.Stop("Run");
                audim.Play("BreathWalk");
            }
            if (r2d.velocity.magnitude > 2.9)
            {
                runSound += Time.deltaTime;
                walkSound = 0;
            }else if (r2d.velocity.magnitude <2.9)
            {
                runSound = 0;
                walkSound += Time.deltaTime;
            }
            if (runSound > 5)
            {
                runSound = 0;
            }
            if (walkSound >5)
            {
                walkSound = 0;
            }
        }
       
        float runnSpeed = 4.7f;
        float walkkSpeed = walkSpeed;
       
        if ((Mathf.Sign(horizontalInput) != Mathf.Sign(direction.x)) && (Mathf.Sign(verticalInput) != Mathf.Sign(direction.y)))
        {
            runnSpeed = 3f;
            walkkSpeed = 1f;
        }else if((horizontalInput != Mathf.Sign(direction.x)) && (verticalInput != Mathf.Sign(direction.y)))
        {
            runnSpeed = 3f;
            walkkSpeed = 1f;
        }
        if (Input.GetButton("Run"))
        {
            Vector2 newVel = new Vector2(horizontalInput, verticalInput);
            newVel.Normalize();
            r2d.velocity = newVel * runnSpeed ;
            cc2d.radius = 1.4f;
            anim.SetFloat("Speed", r2d.velocity.magnitude);
            anim.SetBool("Run", true);
        }
        else 
        {
            Vector2 newVel = new Vector2(horizontalInput, verticalInput);
            newVel.Normalize();
            r2d.velocity = newVel *walkkSpeed ;
            cc2d.radius = 0.8f;
            anim.SetFloat("Speed", r2d.velocity.magnitude);
            anim.SetBool("Run", false);
        }
        if (r2d.velocity.magnitude == 0)
        {
            audim.Stop("Walk");
            audim.Stop("BreathWalk");
            audim.Stop("BreathRun");
            audim.Stop("Run");
            walkSound = 0;
        }

       
    }
    private void rotatePlayer()
    {
        //float rotation = worldPosition* rotationSpeed;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject,4f);
            anim.SetBool("Ded", true);
            audim.Play("Ded");
            transform.GetChild(0).gameObject.SetActive(false);
            r2d.freezeRotation = true;
            r2d.isKinematic = true;
            r2d.velocity = new Vector2 (0f,0f);
            gameObject.GetComponent<Movement>().enabled = false; 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //Debug.Log("wall");
        }
    }
}
