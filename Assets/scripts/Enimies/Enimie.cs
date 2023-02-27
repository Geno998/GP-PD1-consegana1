using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public struct EnimieStats
{
    public int maxHP;
    public int movementSpeed;
    public int scorePerDeath;
    public bool canShoot;
    public bool aimPlayer;
    public Sprite sprite;
    public Transform targetStart;
    public GameObject enimieProjectile;

    public EnimieStats(int _Maxhp, int _MovementSpeed, bool _canShoot, bool _aimPlayer, int _scorePerDeath, Sprite _Sprite, Transform _Target, GameObject _enimieProjectile)  
    {
        maxHP = _Maxhp;
        movementSpeed = _MovementSpeed;
        scorePerDeath = _scorePerDeath;
        sprite = _Sprite;
        targetStart = _Target;
        canShoot = _canShoot;
        aimPlayer = _aimPlayer;
        enimieProjectile = _enimieProjectile;
    }
}

public class Enimie : MonoBehaviour
{
    int scorePerDeath;
    public int currentHP;
    bool dead = false;
    public bool aimAtPlayer;
    public EnimieStats enemiesStats;
    SpriteRenderer renderer;




    [SerializeField] GameObject shooter;
    GameObject enemyProjectile;
    [SerializeField] GameObject enemyProjectileFolder;

    float iFrames;

    float shootTimer = 3;

    float destroyTimer = 0.5f;
    [SerializeField] GameObject ship;
    [SerializeField] GameObject explosionAnim;

    AudioSource audio;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] AudioClip hitAudio;

    BoxCollider2D collider;

    int dropRng;
    [SerializeField] GameObject healthPack;

    void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();

        enemyProjectile = enemiesStats.enimieProjectile;

        currentHP = enemiesStats.maxHP;
        aimAtPlayer = enemiesStats.aimPlayer;
        scorePerDeath = enemiesStats.scorePerDeath;
        renderer.sprite = enemiesStats.sprite;

        if (aimAtPlayer)
        {
            transform.right = enemiesStats.targetStart.position - transform.position;
        }
        else
        {
            transform.right = new Vector2(-1, 0);
        }

    }


    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (!dead)
            {
                if (aimAtPlayer)
                {
                    shooter.transform.right = enemiesStats.targetStart.position - transform.position;
                }
                else
                {
                    shooter.transform.right = new Vector2(-1, 0);
                }

                transform.Translate(new Vector2(1 * Time.deltaTime * enemiesStats.movementSpeed, 0));
            }



            if (currentHP <= 0)
            {
                deafeat();
            }

            Damaged();
            if (enemiesStats.canShoot == true)
            {
                shoot();
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (collision.CompareTag("Player Porojectile"))
            {
                if (iFrames <= 0)
                {

                    iFrames = 0.2f;
                    currentHP--;
                    audio.PlayOneShot(hitAudio, 1);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (collision.CompareTag("Player Porojectile"))
            {
                if (iFrames <= 0)
                {

                    iFrames = 0.2f;
                    currentHP--;
                    audio.PlayOneShot(hitAudio, 1);
                }
            }


            if (collision.CompareTag("Enimie Bounds"))
            {
                Destroy(gameObject);
            }
        }
    }
    void deafeat()
    {
        if (currentHP <= 0)
        {
            if (destroyTimer > 0)
            {
                if (destroyTimer == 0.5f)
                {
                    collider.enabled = false;
                    dead = true;
                    audio.PlayOneShot(hitAudio, 0.5f);
                    audio.PlayOneShot(explosionAudio, 1);
                    dropRng = Random.Range(0, 4);
                    if(dropRng == 0)
                    {
                        Instantiate(healthPack,transform.position,Quaternion.identity);
                    }
                }


                ship.SetActive(false);
                explosionAnim.SetActive(true);

                destroyTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + scorePerDeath);
            }

        }

    }

    void Damaged()
    {
        if (iFrames > 0)
        {
            iFrames -= Time.deltaTime;
            renderer.color = Color.red;
        }
        else
        {
            renderer.color = Color.white;
        }
    }

    void shoot()
    {


        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            shootTimer = 5f;
            Instantiate(enemyProjectile, transform.position, shooter.transform.rotation);
            audio.PlayOneShot(shootAudio, 1);
        }
    }
}
