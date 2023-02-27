using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class S_pMovement : MonoBehaviour
{



    Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] float moveX;
    [SerializeField] float moveY;

    [SerializeField] Vector2 moveDir;

    public List<GameObject> projectiles;
    projectile projectile;
    [SerializeField] float shootRate;
    float timer;
    int currentP;


    bool forward = true;

    inputManager input;


    AudioSource audioSource;
    [SerializeField] AudioClip shootAudio;


    public float energy;
    public float maxEnergy = 100;
    [SerializeField] float chargeSpeed;
    [SerializeField] GameObject chargedBullet;
    [SerializeField] GameObject laser;
    float lazerTime;
    bool lazerOn;


    [SerializeField] Transform cutScenePos;
    Collider2D collider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
        input = gameManager.Instance.input;
    }


    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.off || gameManager.Instance.bossCutscene == gameManager.BossCutscene.fight)
            {
                if (!lazerOn)
                {
                    shoot();
                    Chargedshoot();
                }

                lazerTimer();
                rotatePlayer();
            }
            cutScene();
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.off || gameManager.Instance.bossCutscene == gameManager.BossCutscene.fight)
            {
                movement();
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

    }


    void movement()
    {
        if (input.moveX != 0 || input.moveY != 0)
        {
            moveX = input.moveX;
            moveY = input.moveY;

            moveDir = new Vector2(moveX, moveY).normalized;

            rb.velocity = new Vector2(moveDir.x * speed, moveY * speed);

        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }


    void shoot()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (input.jump)
        {


            if (timer <= 0)
            {


                audioSource.PlayOneShot(shootAudio, 1);
                projectile = projectiles[currentP].GetComponent<projectile>();
                if (forward)
                {
                    projectiles[currentP].transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                }
                else
                {
                    projectiles[currentP].transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                }

                projectiles[currentP].transform.rotation = transform.rotation;
                projectile.on = true;

                if (currentP < projectiles.Count - 1)
                {
                    currentP++;
                }
                else
                {
                    currentP = 0;
                }

                timer = shootRate;

                Debug.Log("fired");
            }

        }
    }


    void Chargedshoot()
    {
        if (input.spetialHold)
        {
            if (energy < 100)
            {
                energy += Time.deltaTime * chargeSpeed;
            }
        }
        else
        {
            if (energy > 0 && energy < 25)
            {
                energy -= Time.deltaTime * chargeSpeed * 5;
            }
        }

        if (input.spetialRelease)
        {
            if (energy >= 25)
            {
                if (energy < 100)
                {
                    Instantiate(chargedBullet, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation);
                    energy = 0;
                }
                else
                {
                    Instantiate(laser, new Vector2(transform.position.x + 1, transform.position.y), transform.rotation, transform);
                    lazerOn = true;
                    lazerTime = 2;
                    energy = 0;
                }
            }
        }


    }

    private void lazerTimer()
    {
        if (lazerTime >= 0)
        {
            lazerTime -= Time.deltaTime;
        }
        else
        {
            lazerOn = false;
        }
    }

    void rotatePlayer()
    {
        if (input.rotate)
        {
            if (forward)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                forward = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                forward = true;
            }
        }

    }


    private void cutScene()
    {
        if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.beginning)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.position = Vector2.MoveTowards(transform.position, cutScenePos.position, Time.deltaTime * 3);
        }

        if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.end)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.Translate(Vector2.right * Time.deltaTime * 3);
        }

        if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.bossDie)
        {
            collider.enabled = false;
        }


            if (transform.position == cutScenePos.position && gameManager.Instance.bossCutscene == gameManager.BossCutscene.beginning)
        {
            gameManager.Instance.bossCutscene = gameManager.BossCutscene.bossComing;
        }

        if (transform.position.x > 12)
        {
            gameManager.Instance.winCon();
        }
    }

}
