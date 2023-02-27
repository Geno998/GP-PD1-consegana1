using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class uIManager : MonoBehaviour
{


    [SerializeField] private Image health;
    [SerializeField] private Image power;
    [SerializeField] private Image bossHealth;

    [SerializeField] private GameObject bossHealthUI;




    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text topScoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text waveText;

    public GameObject uI;
    public GameObject PauseMenu;

    public GameObject info;
    public GameObject mainMenu;

    public float minutes = 3;
    public float seconds;
    public float globalTime;

    Player playerRef;
    inputManager input;
    S_pMovement pMove;
    Boss bossRef;


    int score;
    int highScore;


    bool bossStarted;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + PlayerPrefs.GetInt("score", 0);

            score = PlayerPrefs.GetInt("Score", 0);
            if (PlayerPrefs.GetInt("topScore", 0) < PlayerPrefs.GetInt("score", 0))
            {
                PlayerPrefs.SetInt("topScore", PlayerPrefs.GetInt("score", 0));
            }
        }

        playerRef = gameManager.Instance.playerRef;
        input = gameManager.Instance.input;
        pMove = gameManager.Instance.movement;
        bossRef = gameManager.Instance.bossRef;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            timerFlow();

            pauseGame();
        }

        display();
    }



    void timerFlow()
    {
        globalTime += Time.deltaTime;

        if (minutes >= -1)
        {
            if (seconds > -0.48f)
            {
                seconds -= Time.deltaTime;

            }
            else
            {
                minutes--;
                seconds = 59.49f;
            }
        }

    }

    void display()
    {
        if (timeText != null)
        {
            if (minutes >= 0)
            {
                if (seconds >= 9.5)
                {
                    timeText.text = "TIME: 0" + minutes + ":" + Mathf.RoundToInt(seconds);
                }
                else
                {
                    timeText.text = "TIME: 0" + minutes + ":" + "0" + Mathf.RoundToInt(seconds);
                }
            }
            else
            {
                if (!bossStarted)
                {
                    timeText.text = "TIME: 00:00";
                    waveText.text = "BOSS";
                    gameManager.Instance.bossCutscene = gameManager.BossCutscene.beginning;
                    bossStarted = true;
                }

            }

            if (minutes == 2)
            {
                waveText.text = "WAVE 1";
            }

            if (minutes == 1)
            {
                waveText.text = "WAVE 2";
            }

            if (minutes == 0)
            {
                waveText.text = "WAVE 3";
            }

        }

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + PlayerPrefs.GetInt("score", 0);
            if (PlayerPrefs.GetInt("topScore", 0) < PlayerPrefs.GetInt("Score", 0))
            {
                PlayerPrefs.SetInt("topScore", PlayerPrefs.GetInt("Score", 0));
            }
        }

        if (topScoreText != null)
        {
            topScoreText.text = "TOP SCORE: " + PlayerPrefs.GetInt("topScore", 0);
        }

        if (health != null)
        {
            health.fillAmount = playerRef.hp / playerRef.maxHp;
            power.fillAmount = pMove.energy / pMove.maxEnergy;

            if (gameManager.Instance.bossCutscene == gameManager.BossCutscene.fight)
            {
                bossHealthUI.SetActive(true);
            }
        }

        if (bossHealth != null)
        {

            bossHealth.fillAmount = bossRef.currentHP / bossRef.maxHP;
            Debug.Log("cutrrent boss health " + bossRef.currentHP + " and max " + bossRef.maxHP);
        }

    }

    void pauseGame()
    {
        if (uI != null && PauseMenu != null)
        {
            if (input.pause)
            {
                gameManager.Instance.gameStatus = gameManager.GameStatus.inMenu;

                PauseMenu.SetActive(true);
            }
        }

    }

    public void goToInfo()
    {
        if (mainMenu != null && info != null)
        {
            mainMenu.SetActive(false);
            info.SetActive(true);
        }

    }
    public void exitInfo()
    {
        if (mainMenu != null && info != null)
        {
            mainMenu.SetActive(true);
            info.SetActive(false);
        }
    }

}
