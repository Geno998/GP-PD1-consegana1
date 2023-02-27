using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class projectile : MonoBehaviour
{

    public float speed;
    public bool on;

    public bool piercing;
    public bool isLaer;


    float LazerTime = 2;

    Vector2 startPos;
    Vector2 startEuler;
    Quaternion startRot;


    Transform player;
    void Start()
    {
        startPos = transform.position;
        startEuler = transform.eulerAngles;
        startRot = Quaternion.identity;
        player = transform.parent;
    }


    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (!isLaer)
            {
                if (on)
                {
                    move();
                }
            }
            else
            {
                destroyLazer();
                lazerPos();
            }

        }
    }


    void move()
    {
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
    }

    void destroyLazer()
    {
        if (LazerTime >= 0)
        {
            LazerTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void lazerPos()
    {
        if (player.rotation == startRot)
        {
            transform.position = new Vector2(player.position.x + 0.5f, player.position.y);
        }
        else
        {
            transform.position = new Vector2(player.position.x - 0.5f, player.position.y);
            transform.eulerAngles = player.eulerAngles;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (!isLaer)
            {
                if (CompareTag("Player Porojectile"))
                {
                    if (!piercing)
                    {
                        if (!collision.CompareTag("Player") && !collision.CompareTag("Enimie Projectile") && !collision.CompareTag("Player Porojectile") && !collision.CompareTag("Health Pack"))
                        { 
                            on = false;
                            transform.position = startPos;
                            transform.eulerAngles = startEuler;
                        }
                    }
                    else
                    {
                        if (collision.CompareTag("Enimie Bounds"))
                        {
                            Destroy(gameObject);
                        }
                    }

                }

                if (CompareTag("Enimie Projectile"))
                {
                    if (!collision.CompareTag("Enimie") && !collision.CompareTag("Player Porojectile") && !collision.CompareTag("Enimie Projectile") && !collision.CompareTag("Health Pack"))
                    {
                        Destroy(gameObject);
                    }
                }

                Debug.Log("collided");
            }
        }

    }

}
