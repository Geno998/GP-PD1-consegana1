using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class inputManager : MonoBehaviour
{

    public float moveX;
    public float moveY;

    public bool jump = false;
    public bool rotate = false;

    public bool pause = false;
    public bool spetialHold = false;
    public bool spetialRelease = false;
    void Update()
    {
        if (gameManager.Instance.gameStatus == gameManager.GameStatus.running)
        {
            gameInputs();
        }
        menuInputs();
    }

    void gameInputs()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        jump = Input.GetButton("Jump");
        rotate = Input.GetButtonDown("Rotate");
        spetialHold = Input.GetButton("Spetial");
        spetialRelease = Input.GetButtonUp("Spetial");
    }

   void menuInputs()
    {
        pause = Input.GetButtonDown("Pause");
    }
}
