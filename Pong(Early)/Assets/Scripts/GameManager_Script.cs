using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject waitingMenu;
    [SerializeField] private GameObject gameStage;
    [SerializeField] private GameObject readyStage;
    [SerializeField] private GameObject backCount;
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject rightPlayer;
    [SerializeField] private GameObject leftPlayer;
    [SerializeField] private TMP_Text rightButtonText;
    [SerializeField] private TMP_Text leftButtonText;
    [SerializeField] private GameObject rightImage;
    [SerializeField] private GameObject leftImage;
    [SerializeField] private GameObject endGame;
    private bool rightPlayer_status = false;
    private bool leftPlayer_status = false;
    private int? side;
    [SerializeField] private TMP_Text name_right;
    [SerializeField] private TMP_Text name_left;
    [SerializeField] private GameObject rightPlayerBtn;
    [SerializeField] private GameObject leftPlayerBtn;
    [SerializeField] private Sprite checkMark;
    [SerializeField] private Sprite cross;
    private Client clientScript;
    private BallScript _ballScript;
    private ContentScript _content;

    private void Awake()
    {
        ball.SetActive(false);
        client = GameObject.Find("TCP_Client");
        client.GetComponent<Client>().SetGameManager(gameObject);
        clientScript = client.GetComponent<Client>();
        WaitingStage_Off();
        ReadyStage_On();
        client.GetComponent<Client>().RequestSide();
        // brackets.GetComponent<CanvasGroup>().alpha = 0f;
        // brackets.GetComponent<CanvasGroup>().interactable = false;
        gameStage.SetActive(false);
        _ballScript = ball.GetComponent<BallScript>();
        _content = GetComponent<ContentScript>();
        endGame.SetActive(false);
    }
    
    private void WaitingStage_On()
    {
        waitingMenu.GetComponent<CanvasGroup>().alpha = 1f;
        waitingMenu.GetComponent<CanvasGroup>().interactable = true;
        waitingMenu.SetActive(true);
    }

    private void WaitingStage_Off()
    {
        waitingMenu.GetComponent<CanvasGroup>().alpha = 0f;
        waitingMenu.GetComponent<CanvasGroup>().interactable = false;
        waitingMenu.SetActive(false);
    }

    public void ReadyStage_On()
    {
        readyStage.GetComponent<CanvasGroup>().alpha = 1f;
        readyStage.GetComponent<CanvasGroup>().interactable = true;
        readyStage.SetActive(true);
        WaitingStage_Off();
    }

    private void ReadyStage_Off()
    {
        readyStage.GetComponent<CanvasGroup>().alpha = 0f;
        readyStage.GetComponent<CanvasGroup>().interactable = false;
        readyStage.SetActive(false);
    }

    public void StartGame()
    {
        // brackets.GetComponent<CanvasGroup>().alpha = 1f;
        // brackets.GetComponent<CanvasGroup>().interactable = true;
        gameStage.SetActive(true);
        ReadyStage_Off();
        switch (side)
        {
            case 0:
                rightPlayer.GetComponent<Player_Script>().StartListenForKeys();
                //rightPlayer.GetComponent<Player_Script>().SetX_Pos(11.2f);
                break;
            case 1:
                leftPlayer.GetComponent<Player_Script>().StartListenForKeys();
                //leftPlayer.GetComponent<Player_Script>().SetX_Pos(-11.2f);
                break;
        }
        clientScript.GameIsStart();
        ball.SetActive(true);
    }

    public void ExitGame()
    {
        client.GetComponent<Client>().ExitGame();
    }
    
    private void CheckPlayersStatus()
    {
        if (leftPlayer_status && rightPlayer_status)
        {
            backCount.SetActive(true);
            backCount.GetComponent<Animator>().enabled = true;
        }
        else
        {
            backCount.SetActive(false);
            backCount.GetComponent<Animator>().enabled = false;
        }
    }

    public void RightPlayerStatus()
    {
        if (!rightPlayer_status)
        {
            rightPlayer_status = true;
            //client.GetComponent<Client>().SendReadyness(rightPlayer_status);
            CheckPlayersStatus();
        }
        else
        {
            rightPlayer_status = false;
            //client.GetComponent<Client>().SendReadyness(rightPlayer_status);
            CheckPlayersStatus();
        }
    }

    public void LeftPlayerStatus()
    {
        if (!leftPlayer_status)
        {
            leftPlayer_status = true;
            //client.GetComponent<Client>().SendReadyness(leftPlayer_status);
            CheckPlayersStatus();
        }
        else
        {
            leftPlayer_status = false;
            //client.GetComponent<Client>().SendReadyness(leftPlayer_status);
            CheckPlayersStatus();
        }
    }

    public void SetOtherPlayerStatus(bool isReady)
    {
        if (side == 0)
        {
            leftPlayer_status = isReady;
            CheckPlayersStatus();
        }
        else
        {
            rightPlayer_status = isReady;
            CheckPlayersStatus();
        }
    }

    public void SetSide(int _side, string enemyName)
    {
        side = _side;
        switch (side)
        {
            case 0:
                rightPlayer.GetComponent<Player_Script>().enabled = true;
                leftPlayer.GetComponent<Player_Script>().enabled = false;
                name_right.enabled = true;
                name_left.enabled = true;
                name_right.text = client.GetComponent<Client>().name;
                name_left.text = enemyName;
                rightPlayerBtn.SetActive(true);
                break;
            case 1:
                leftPlayer.GetComponent<Player_Script>().enabled = true;
                rightPlayer.GetComponent<Player_Script>().enabled = false;
                name_left.enabled = true;
                name_right.enabled = true;
                name_left.text = client.GetComponent<Client>().name;
                name_right.text = enemyName; 
                leftPlayerBtn.SetActive(true);
                break;
        }
        clientScript.LeaderList();
    }
    
    public void Accept_Ready()
    {
        switch (side)
        {
            case 0:
                rightPlayer_status = true;
                rightImage.GetComponent<Image>().sprite = checkMark;
                rightButtonText.text = "Not Ready";
                CheckPlayersStatus();
                break;
            case 1:
                leftPlayer_status = true;
                leftImage.GetComponent<Image>().sprite = checkMark;
                leftButtonText.text = "Not Ready";
                CheckPlayersStatus();
                break;
        }
    }

    public void Accept_NotReady()
    {
        switch (side)
        {
            case 0:
                rightPlayer_status = false;
                rightImage.GetComponent<Image>().sprite = cross;
                rightButtonText.text = "Not Ready";
                CheckPlayersStatus();
                break;
            case 1:
                leftImage.GetComponent<Image>().sprite = cross;
                leftButtonText.text = "Not Ready";
                leftPlayer_status = false;
                CheckPlayersStatus();
                break;
        }
    }

    public void Accept_Enemy_Ready()
    {
        switch (side)
        {
            case 0:
                leftPlayer_status = true;
                leftImage.GetComponent<Image>().sprite = checkMark;
                CheckPlayersStatus();
                break;
            case 1:
                rightPlayer_status = true;
                rightImage.GetComponent<Image>().sprite = checkMark;
                CheckPlayersStatus();
                break;
        }
    }

    public void Accept_Enemy_NotReady()
    {
        switch (side)
        {
            case 0:
                leftPlayer_status = false;
                leftImage.GetComponent<Image>().sprite = cross;
                CheckPlayersStatus();
                break;
            case 1:
                rightPlayer_status = false;
                rightImage.GetComponent<Image>().sprite = cross;
                CheckPlayersStatus();
                break;
        }
    }

    public void SendReadyness()
    {
        if (side == 0)
        {
            client.GetComponent<Client>().SendReadyness(!rightPlayer_status);
        }
        else
        {
            
            client.GetComponent<Client>().SendReadyness(!leftPlayer_status);
        }
    }

    public void Send_Request_For_Move_Up()
    {
        clientScript.MoveUp();
    }

    public void Send_Request_For_Move_Down()
    {
        clientScript.MoveDown();
    }

    public void Accept_Request_For_Move_Up(float position, float dir, float speed)
    {
        switch (side)
        {
            case 0:
                rightPlayer.GetComponent<Player_Script>().Move(position, dir, speed);
                break;
            case 1:
                leftPlayer.GetComponent<Player_Script>().Move(position, dir, speed);
                break;
        }
    }

    public void Accept_Request_For_Move_Down(float position, float target, float speed)
    {
        switch (side)
        {
            case 0:
                rightPlayer.GetComponent<Player_Script>().Move(position, target, speed);
                break;
            case 1:
                leftPlayer.GetComponent<Player_Script>().Move(position, target, speed);
                break;
        }
    }

    public void Accept_Request_For_Move_Enemy(float position, float target, float speed)
    {
        switch (side)
        {
            case 0:
                leftPlayer.GetComponent<Player_Script>().Move(position, target, speed);
                break;
            case 1:
                rightPlayer.GetComponent<Player_Script>().Move(position, target, speed);
                break;
        }
    }

    public void SetBallPos(float x, float y)
    {
        _ballScript.SetPosition(x, y);
    }

    public void FormLeaderList(string[] arguments)
    {
        for (int i = 1; i < arguments.Length; i++)
        {
            string[] info = arguments[i].Split("-");
            _content.AddLeader(info[0], Convert.ToInt32(info[1]));
        }
        _content.SpawnLeaders();
    }

    public void Scored(string[] arguments)
    {
        switch (side)
        {
            case 0:
                gameStage.GetComponent<CountScript>().AddRight();
                rightPlayer.GetComponent<Player_Script>().ResetPos(Convert.ToSingle(arguments[3]));
                break;
            case 1:
                gameStage.GetComponent<CountScript>().AddLeft();
                leftPlayer.GetComponent<Player_Script>().ResetPos(Convert.ToSingle(arguments[3]));
                break;
        }
        _ballScript.SetPosition(Convert.ToSingle(arguments[1]), Convert.ToSingle(arguments[2]));
        
    }

    public void GotScored(string[] arguments)
    {
        switch (side)
        {
            case 0:
                gameStage.GetComponent<CountScript>().AddLeft();
                rightPlayer.GetComponent<Player_Script>().ResetPos(Convert.ToSingle(arguments[3]));
                break;
            case 1:
                gameStage.GetComponent<CountScript>().AddRight();
                leftPlayer.GetComponent<Player_Script>().ResetPos(Convert.ToSingle(arguments[3]));
                break;
        }
        _ballScript.SetPosition(Convert.ToSingle(arguments[1]), Convert.ToSingle(arguments[2]));
    }

    public void ChangeBallDirection(string[] arguments)
    {
        _ballScript.StartMove(
            new Vector3(Convert.ToSingle(arguments[1]), Convert.ToSingle(arguments[2])),
            new Vector3(Convert.ToSingle(arguments[3]), Convert.ToSingle(arguments[4])),
            Convert.ToSingle(arguments[5]));
    }

    public void ReceiveEndGameMessage(string status)
    {
        endGame.SetActive(true);
        gameStage.SetActive(false);
        switch (side)
        {
            case 0:
                rightPlayer.GetComponent<Player_Script>().EndListenForKey();
                //rightPlayer.GetComponent<Player_Script>().SetX_Pos(11.2f);
                break;
            case 1:
                leftPlayer.GetComponent<Player_Script>().EndListenForKey();
                //leftPlayer.GetComponent<Player_Script>().SetX_Pos(-11.2f);
                break;
        }
        ball.SetActive(false);
        switch (status)
        {
            case "win":
                endGame.GetComponent<EndGameScript>().ShowMessage(true);
                break;
            case "lost":
                endGame.GetComponent<EndGameScript>().ShowMessage(false);
                break;
        }
    }
}
