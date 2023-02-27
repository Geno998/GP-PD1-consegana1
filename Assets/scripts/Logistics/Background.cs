using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] int bGPos;
    [SerializeField] int bGNum;

    [SerializeField] List<Transform> bG;

    bool bossSpawned = false;


    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            if (!bossSpawned)
            {
                if (bGNum == 0)
                {
                    calculatePos(2, 1);
                }
                else if (bGNum == 1)
                {
                    calculatePos(0, 2);
                }
                else if (bGNum == 2)
                {
                    calculatePos(1, 0);
                }
            }

            if(gameManager.Instance.bossCutscene == gameManager.BossCutscene.fight)
            {
                bossSpawned = true;
            }

        }
    }


    void calculatePos(int beforeTile, int nextTile)
    {
        if (bGPos == 0)
        {
            if (transform.position.x > -48)
            {
                transform.Translate(new Vector2(-4 * Time.deltaTime, 0));
            }
            else if (transform.position.x <= -48)
            {
                bGPos = 2;
            }
        }

        if (bGPos == 1)
        {
            if (bG[beforeTile].position.x > -48)
            {
                transform.position = new Vector2(bG[beforeTile].position.x + 24, 0);
            }
            else if (bG[beforeTile].position.x <= -48)
            {
                bGPos--;
            }
        }

        if (bGPos == 2)
        {
            if (bG[nextTile].position.x > -48)
            {
                transform.position = new Vector2(bG[beforeTile].position.x + 24, 0);
            }
            else if (bG[nextTile].position.x <= -48)
            {
                bGPos--;
            }
        }
    }




    // non va bene (Con il Lag o spammando pausa lo sfondo si sfasa)


    //void Update()
    //{
    //    if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
    //    {
    //        if (transform.position.x > -48)
    //        {
    //            transform.Translate(new Vector2(-4 * Time.deltaTime, 0));
    //        }
    //        else if (transform.position.x <= -48)
    //        {
    //            transform.position = new Vector2(48, 0);
    //        }
    //    }
    //}
}
