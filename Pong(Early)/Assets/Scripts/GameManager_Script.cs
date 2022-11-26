using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour
{
    [SerializeField] private GameObject waitingMenu;
    [SerializeField] private GameObject brackets;
    [SerializeField] private GameObject readyStage;
    [SerializeField] private GameObject backCount;
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject rightPlayer;
    [SerializeField] private GameObject leftPlayer;
    [SerializeField] private TMP_Text rightButtonText;
    [SerializeField] private TMP_Text leftButtonText;
    [SerializeField] private GameObject rightImage;
    [SerializeField] private GameObject leftImage;
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

    private void Awake()
    {
        client = GameObject.Find("TCP_Client");
        client.GetComponent<Client>().SetGameManager(gameObject);
        clientScript = client.GetComponent<Client>();
        WaitingStage_Off();
        ReadyStage_On();
        client.GetComponent<Client>().RequestSide();
        brackets.GetComponent<CanvasGroup>().alpha = 0f;
        brackets.GetComponent<CanvasGroup>().interactable = false;
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
        brackets.GetComponent<CanvasGroup>().alpha = 1f;
        brackets.GetComponent<CanvasGroup>().interactable = true;
        ReadyStage_Off();
        switch (side)
        {
            case 0:
                rightPlayer.GetComponent<Player_Script>().StartListenForKeys();
                rightPlayer.GetComponent<Player_Script>().SetX_Pos(11.2f);
                break;
            case 1:
                leftPlayer.GetComponent<Player_Script>().StartListenForKeys();
                leftPlayer.GetComponent<Player_Script>().SetX_Pos(-11.2f);
                break;
        }
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

    public void Accept_Request_For_Move_Enemey(float position, float target, float speed)
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
}
