using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class Recieve : MonoBehaviour
{
    [SerializeField] private Client client;
    private enum Commands
    {
        id,
        both_con,
        rdy,
        side
    }
    private Socket socket;
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }

    /*private void Update()
    {
        RecieveMessage();
    }*/
    

    public void RecieveMessage()
    {
        while (true)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            String[] param = res.Split(" ");

            switch ((Commands)Enum.Parse(typeof(Commands), param[0]))
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
                case Commands.side:
                    GetSide(param);
                    break;
            }
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

    private void GetSide(string[] parametrs)
    {
        client.GetSide(parametrs);
    }
}
