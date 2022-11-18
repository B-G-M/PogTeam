using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Recieve_Send_scrpt : MonoBehaviour
{
    [SerializeField] private Client client;
    private enum Commands
    {
        id,
        both_con,
        rdy
    }
    private Socket socket;
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }
    
    public void RecieveMessage()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] param = res.Split(" ");

        switch ((Commands) Enum.Parse(typeof(Commands), param[0]))
        {
            case Commands.id:
                GetID(param);
                break;
            case Commands.both_con:
                Both_Connected();
                break;
            case Commands.rdy:
                Readyness(param);
                break;
            
                    
        }
    }
    public  void SendMessage(String outMessage)
    {
        var message = outMessage;
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
    }
    public void ExitGame()
    {
        if (socket.Connected)
        {
            socket.Close();
        }
    }

    private void GetID(string[] parametrs)
    {
        client.GetId(parametrs);
    }

    private void Both_Connected()
    {
        client.Both_Connected();
    }

    private void Readyness(string[] parametrs)
    {
        client.Readyness(parametrs);
    }
}
