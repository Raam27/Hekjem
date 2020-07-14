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
    // Start is called before the first frame update
    void Start()
    {
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
            chase();
            if (distance > 6)
            {
                player = null;
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
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
        }
    }

    private void rotate()
    {
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        transform.up = direction;
    }
    private void chase()
    {
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed*Time.deltaTime);
        rotate();
    }
    private void move()
    {
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * patrolSpeed;
    }
}
