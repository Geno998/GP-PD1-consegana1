using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enimieSpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] Transform[] sideSpawnPos;
    [SerializeField] float spawnRate;
    [SerializeField] int spawnSide;
    //[SerializeField] private MyStruct myStruct = new MyStruct(1, 3, new Vector3(1, 1, 1));
    //[SerializeField] private EnemyStats stats = new EnemyStats(1);//assegno solo alla prima, cioè a

    [SerializeField] EnimieStats[] possibleStats;



    float spawnTimer;

    uIManager uIM;


    private void Start()
    {
        uIM = gameManager.Instance.uIMan;
    }
    private void Update()
    {

        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (uIM.minutes == 2)
            {
                spawnFase(0,1,0.5f);
            }
            else if (uIM.minutes == 1)
            {
                spawnFase(2,3,2);
            }
            else if (uIM.minutes == 0 && uIM.seconds >= 10)
            {
                spawnFase(4,5,1);
            }
        }
    }

    void spawnFase(int enemy1, int enemy2, float spawnMultiplyer)
    {

        spawnTimer += Time.deltaTime;

        if (spawnSide == 0)
        {
            if (spawnTimer >= spawnRate * spawnMultiplyer)
            {

                int randSpawner = Random.Range(0, spawnPos.Length);
                Vector2 spawnPosition = spawnPos[randSpawner].position;



                GameObject enimie = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, spawnPos[randSpawner]);
                enimie.GetComponent<Enimie>().enemiesStats = possibleStats[enemy1];

                spawnTimer = 0;
                spawnSide = Random.Range(0, 2);
            }

        }
        else
        {
            if (spawnTimer >= spawnRate * spawnMultiplyer)
            {

                Debug.Log("left");
                int randSpawner = Random.Range(0, sideSpawnPos.Length);
                Vector2 spawnPosition = sideSpawnPos[randSpawner].position;


                GameObject enimie = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, sideSpawnPos[randSpawner]);
                enimie.GetComponent<Enimie>().enemiesStats = possibleStats[enemy2];

                spawnTimer = 0;
                spawnSide = Random.Range(0, 2);
            }
        }

    }

}