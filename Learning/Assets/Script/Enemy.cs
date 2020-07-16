using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D r2d;
    public float distance;
    public int speed;
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 3f;
    private float patrolSpeed = 1f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;
    public float xMin, xMax, yMin, yMax;
    public float a = 0;
    public Animator anim;
    bool chase;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
        latestDirectionChangeTime = 0f;
        move();

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
            Chase();
            chase = true;
            if (distance > 6)
            {
                player = null;
                chase = false;
            }
        }
        else if (player == null)
        {
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                move();
            }
            Vector2 gerak = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime),
                transform.position.y + (movementPerSecond.y * Time.deltaTime));
            transform.position = new Vector2(Mathf.Clamp(gerak.x, xMin, xMax), Mathf.Clamp(gerak.y, yMin, yMax));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Flashlight")
        {
            player = collision.gameObject.transform.parent.gameObject;
        }
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
        }
    }

    private void Rotate()
    {
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        transform.up = -direction;
    }
    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed*Time.deltaTime);
        Rotate();
        anim.SetBool("Chase", true);
    }
    private void move()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * patrolSpeed;
        transform.up = -movementDirection;
        anim.SetBool("Chase", false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && chase==true)
        {
            Vector2[] spawnPos = new Vector2[4];
            spawnPos[0] = new Vector2(player.transform.position.x, player.transform.position.y+2f);
            spawnPos[1] = new Vector2(player.transform.position.x+2f, player.transform.position.y + 2f);
            spawnPos[2] = new Vector2(player.transform.position.x-2f, player.transform.position.y );
            spawnPos[3] = new Vector2(player.transform.position.x - 2f, player.transform.position.y - 2f);
            bool kosong = false;
            a += Time.deltaTime;
            Debug.Log(a);
            if (a > 5)
            {
                foreach(Vector2 spawn in spawnPos)
                {
                    Vector2 spawn1 = new Vector2(Mathf.Clamp(spawn.x, xMin, xMax), Mathf.Clamp(spawn.y, yMin, yMax));
                    if (Physics2D.OverlapCircle(spawn1, 1.2f))
                    {
                        Debug.Log(spawn1);
                    }
                    else
                    {
                        Debug.Log("kesatu");
                        transform.position = spawn1;
                        return;
                    }
                }
                while (kosong==false)
                {
                    Vector2 randomSpawn = new Vector2(player.transform.position.x +Random.Range(-4f,4f), player.transform.position.y + Random.Range(-4f, 4f));
                    if (Physics2D.OverlapCircle(randomSpawn, 1.2f))
                    {
                      
                    }
                    else
                    {
                        Debug.Log("kedua");
                        transform.position = new Vector2(Mathf.Clamp(randomSpawn.x, xMin, xMax), Mathf.Clamp(randomSpawn.y, yMin, yMax));
                        return;
                    }
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            a = 0;
        }
    }
}
