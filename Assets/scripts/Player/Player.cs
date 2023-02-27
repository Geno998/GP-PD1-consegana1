using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHp;
    public float hp;

    float flashcicle;
    [SerializeField] float flashTime;
    int flashNum;
    bool flashed;
    SpriteRenderer renderer;

    [SerializeField] GameObject explosions;
    [SerializeField] GameObject ship;
    float deathAnimTime;

    AudioSource audio;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioClip hitAudio;

    private void Awake()
    {
        hp = maxHp;
    }

    void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            iFramesCalc();
        }

        death();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (!collision.CompareTag("Player Porojectile") && !collision.CompareTag("Health Pack"))
            {
                if (flashNum == 0)
                {
                    damage();
                }
            }

            if (collision.CompareTag("Health Pack"))
            {
                Destroy(collision.gameObject);
                if(hp < maxHp)
                {
                    hp++;
                }
            }


        }

    }


    private void death()
    {
        if (hp > 0)
        {

            deathAnimTime = 2;
        }
        else
        {
            if (deathAnimTime == 2)
            {
                audio.PlayOneShot(explosionAudio, 1);
            }
            gameManager.Instance.gameStatus = gameManager.GameStatus.inAnim;

            ship.SetActive(false);
            explosions.SetActive(true);
            deathAnimTime -= Time.deltaTime;
        }

        if (deathAnimTime <= 0)
        {
            gameManager.Instance.gameStatus = gameManager.GameStatus.inMenu;
            gameManager.Instance.loseCon();
        }


    }

    private void damage()
    {
        if (flashNum == 0)
        {
            audio.PlayOneShot(hitAudio, 1);
            hp--;
            flashNum = 10;

        }

    }


    void iFramesCalc()
    {
        if (flashNum > 0)
        {
            if (flashcicle <= 0)
            {

                flashNum--;
                flashcicle = flashTime;
                if (!flashed)
                {
                    flashed = true;
                    renderer.color = Color.yellow;
                }
                else
                {
                    flashed = false;
                    renderer.color = Color.white;
                }
            }
            else
            {
                flashcicle -= Time.deltaTime;
            }
        }
        else
        {
            flashed = false;
            renderer.color = Color.white;
        }
    }
}
