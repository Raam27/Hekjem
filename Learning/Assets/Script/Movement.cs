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
    AudioManager audim;
    Vector2 direction;
    float horizontalInput;
    float verticalInput;
    float rotateInput;
    Vector3 worldPosition;
    float walkSound;

    // Start is called before the first frame update
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        cc2d = gameObject.GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        audim = FindObjectOfType<AudioManager>();
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
            if (walkSound ==0 )
            {
                Debug.Log("nyala");
                audim.Play("Walk");
            }
            walkSound += Time.deltaTime;
            if (walkSound >5)
            {
                walkSound = 0;
            }
        }
       
        float runnSpeed = 4.7f;
        float walkkSpeed = walkSpeed;
        Debug.Log(Mathf.Sign(horizontalInput));
        if (Mathf.Sign(horizontalInput) != Mathf.Sign(direction.x) || Mathf.Sign(verticalInput) != Mathf.Sign(direction.y)){
            runnSpeed = 3f;
            walkkSpeed = 1f;
        }
        if (Input.GetButton("Run"))
        {
            r2d.velocity = new Vector2(horizontalInput * runnSpeed, verticalInput * runnSpeed);
            cc2d.radius = 1.4f;
            anim.SetFloat("Speed", r2d.velocity.magnitude);
            anim.SetBool("Run", true);
            audim.SpeedUp("Walk");
        }
        else 
        {
            r2d.velocity = new Vector2(horizontalInput * walkkSpeed, verticalInput * walkkSpeed);
            cc2d.radius = 0.8f;
            anim.SetFloat("Speed", r2d.velocity.magnitude);
            anim.SetBool("Run", false);
            audim.SlowDown("Walk");
        }
        if (r2d.velocity.magnitude == 0)
        {
            audim.Stop("Walk");
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
            Destroy(gameObject);
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
