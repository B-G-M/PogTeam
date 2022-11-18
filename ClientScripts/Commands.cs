using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Commands : MonoBehaviour
{
    [SerializeField] private Recieve_Send_scrpt _recieveSendScrpt;
    
    public void ExitGame()
    {
        _recieveSendScrpt.ExitGame();
    }

    public void GetAuthentification(String login, String password)
    {
        String message = $"auth {login} {password}";
        _recieveSendScrpt.SendMessage(message);
    }

    public void SendSide(int id, int side)
    {
        string message = $"ch_side {id} {side}";
        _recieveSendScrpt.SendMessage(message);
    }

    public void SendReadyness(int id, bool ready)
    {
        string message = $"rdy {id} {ready}";
        _recieveSendScrpt.SendMessage(message);
    }
}
