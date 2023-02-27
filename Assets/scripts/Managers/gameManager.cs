using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class gameManager : MonoBehaviour
{




    private static gameManager _instance;
    public static gameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }



    private Player _playerRef;
    private S_pMovement _movement;
    private enimieSpawner _enimieSpawner;
    private inputManager _inputM;
    private uIManager _uIMan;
    private sceneManager _sceneM;
    private Boss _bossRef;



    public Player playerRef { get { return _playerRef; } set { _playerRef = value; } }
    public S_pMovement movement { get { return _movement; } set { _movement = value; } }
    public enimieSpawner Spawner { get { return _enimieSpawner; } set { _enimieSpawner = value; } }
    public inputManager input { get { return _inputM; } set { _inputM = value; } }
    public uIManager uIMan { get { return _uIMan; } set { _uIMan = value; } }
    public sceneManager sceneM { get { return _sceneM; } set { _sceneM = value; } }
    public Boss bossRef { get { return _bossRef; } set { _bossRef = value; } }


    public enum GameStatus
    {
        Stand,
        running,
        inMenu,
        inAnim,
    }

    public GameStatus gameStatus = GameStatus.inMenu;



    public enum BossCutscene
    {
        off,
        beginning,
        bossComing,
        fight,
        bossDie,
        end,
    }

    public BossCutscene bossCutscene = BossCutscene.off;



    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRef = FindObjectOfType<Player>();
        movement = FindObjectOfType<S_pMovement>();
        input = FindObjectOfType<inputManager>();
        uIMan = FindObjectOfType<uIManager>();
        sceneM = FindObjectOfType<sceneManager>();
        bossRef = FindObjectOfType<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Instance.gameStatus == GameStatus.running)
        {
            pauseGame();
        }

    }


    void pauseGame()
    {
        if (input.pause)
        {
            Instance.gameStatus = GameStatus.inMenu;
            uIMan.uI.SetActive(false);
            uIMan.PauseMenu.SetActive(true);
        }
    }

    public void continueButton()
    {
        Instance.gameStatus = GameStatus.running;
        uIMan.uI.SetActive(true);
        uIMan.PauseMenu.SetActive(false);
    }

    public void restartButton()
    {
        sceneM.loadScne(1);
        PlayerPrefs.SetInt("score", 0);
    }

    public void menuButton()
    {
        sceneM.loadScne(0);
        PlayerPrefs.SetInt("score", 0);
    }

    public void loseCon()
    {
        sceneM.loadScne(2);
    }

    public void winCon()
    {
        sceneM.loadScne(3);
    }

    public void exitButton()
    {
        Application.Quit();
    }

    public void resetScoreButton()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("topScore", 0);
    }

}
