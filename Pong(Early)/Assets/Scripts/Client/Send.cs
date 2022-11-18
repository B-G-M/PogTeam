using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Send : MonoBehaviour
{
    private Socket socket;
    
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }
    

    public void GetAuthentification(String login, String password)
    {
        String message = $"auth {login} {password}";
        
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void SendSide(int id, int side)
    {
        string message = $"ch_side {id} {side}";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }

    public void SendReadyness(int id, bool ready)
    {
        string message = $"rdy {id} {ready}";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }
}
