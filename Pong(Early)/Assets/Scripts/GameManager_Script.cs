using System.Collections;
using System.Collections.Generic;
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
    private bool rightPlayer_status = false;
    private bool leftPlayer_status = false;
    private int? side;
    [SerializeField] private TMP_Text name_right;
    [SerializeField] private TMP_Text name_left;
    [SerializeField] private Button rightPlayerBtn;
    [SerializeField] private Button leftPlayerBtn;

    private void Awake()
    {
        WaitingStage_On();
        ReadyStage_Off();
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
            client.GetComponent<Client>().SendReadyness(rightPlayer_status);
            CheckPlayersStatus();
        }
        else
        {
            rightPlayer_status = false;
            client.GetComponent<Client>().SendReadyness(rightPlayer_status);
            CheckPlayersStatus();
        }
    }

    public void LeftPlayerStatus()
    {
        if (!leftPlayer_status)
        {
            leftPlayer_status = true;
            client.GetComponent<Client>().SendReadyness(leftPlayer_status);
            CheckPlayersStatus();
        }
        else
        {
            leftPlayer_status = false;
            client.GetComponent<Client>().SendReadyness(leftPlayer_status);
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

    public void SetSide(int _side)
    {
        side = _side;
        if (side == 0)
        {
            rightPlayer.GetComponent<Player_Script>().enabled = true;
            leftPlayer.GetComponent<Player_Script>().enabled = false;
            name_right.enabled = true;
            name_right.text = client.GetComponent<Client>().name;
        }
        else
        {
            leftPlayer.GetComponent<Player_Script>().enabled = true;
            rightPlayer.GetComponent<Player_Script>().enabled = false;
            name_left.enabled = true;
            name_left.text = client.GetComponent<Client>().name;
        }
    }

    public void BothConnected()
    {
        ReadyStage_On();
        if (side == 0)
        {
            rightPlayerBtn.enabled = true;
        }
        else
        {
            leftPlayerBtn.enabled = true;
        }
    }

   
}
