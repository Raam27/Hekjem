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
    public int runSpeed;
    public int walkSpeed;
    public CircleCollider2D cc2d;
    public Animator anim;

    float horizontalInput;
    float verticalInput;
    float rotateInput;
    Vector3 worldPosition;

    // Start is called before the first frame update
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
        if (Input.GetButton("Run"))
        {
            r2d.velocity = new Vector2(horizontalInput * runSpeed, verticalInput * runSpeed);
            cc2d.radius = 1.5f;
            anim.SetFloat("Speed", r2d.velocity.magnitude);
            anim.SetBool("Run", true);
        }
        else 
        {
            r2d.velocity = new Vector2(horizontalInput * walkSpeed, verticalInput * walkSpeed);
            cc2d.radius = 1.1f;
            anim.SetFloat("Speed", r2d.velocity.magnitude);
            anim.SetBool("Run", false);
        }
    }
    private void rotatePlayer()
    {
        //float rotation = worldPosition* rotationSpeed;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
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
