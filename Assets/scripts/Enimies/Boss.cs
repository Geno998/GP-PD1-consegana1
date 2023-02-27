using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] Transform bossPos;
    [SerializeField] Transform bossEndPos;


    int scorePerDeath = 10000;
    public float currentHP;
    public float maxHP;





    [SerializeField] List<GameObject> shooter;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] GameObject enemyProjectileFolder;
    [SerializeField] Transform rot;

    float iFrames;



    [SerializeField] GameObject explosionAnim;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] AudioClip hitAudio;

    [SerializeField] SpriteRenderer renderer;


    float shootTimer;

 

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        bossCutscenes();

        defeat();
        if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.fight)
        {
            Damaged();
            attack();
         
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

                    iFrames = 0.3f;
                    currentHP--;
                    GetComponent<AudioSource>().PlayOneShot(hitAudio, 0.3f);
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

                    iFrames = 0.3f;
                    currentHP--;
                    GetComponent<AudioSource>().PlayOneShot(hitAudio, 0.3f);
                }
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
            renderer.color = Color.gray;
        }
    }

    private void bossCutscenes()
    {
        if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.bossComing)
        {
            transform.position = Vector2.MoveTowards(transform.position, bossPos.position, Time.deltaTime * 5);
        }

        if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.bossDie)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            transform.position = Vector2.MoveTowards(transform.position, bossEndPos.position, Time.deltaTime * 1);
        }

        if (transform.position == bossPos.position)
        {
            gameManager.Instance.bossCutscene = gameManager.BossCutscene.fight;
        }

        if (transform.position == bossEndPos.position)
        {
            gameManager.Instance.bossCutscene = gameManager.BossCutscene.end;
        }
    }

    private void defeat()
    {
        if (currentHP == 0)
        {
            gameManager.Instance.bossCutscene = gameManager.BossCutscene.bossDie;
            PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + scorePerDeath);
            explosionAnim.SetActive(true);
            audioSource.PlayOneShot(explosionAudio);
            currentHP = -1;
        }
    }


    private void attack()
    {

        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            Instantiate(enemyProjectile, shooter[Random.Range(0, shooter.Count)].transform.position, rot.rotation);
            shootTimer = 0.3f;

        }

    }
}
