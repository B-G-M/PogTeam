using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class Recieve : MonoBehaviour
{
    [SerializeField] private static Client client;
    [SerializeField] private static ThreadManager _threadManager;
    private static Socket socket;
    private static Thread testThread = null;
    
    private enum Commands
    {
        id,
        both_con,
        rdy,
        side
    }

    private void Awake()
    {
        client = GetComponent<Client>();
        _threadManager = GetComponent<ThreadManager>();
    }
    
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }

    public void StartListening()
    {
        testThread = new Thread(new ThreadStart(ThreadAction));
        testThread.Start();
    }
    private void ThreadAction() 
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        String[] param = res.Split(" ");

        switch ((Commands)Enum.Parse(typeof(Commands), param[0]))
        {
            case Commands.id:
                _threadManager.ExecuteOnMainThread(() =>
                {
                    GetID(param);
                });
                break;
            case Commands.both_con:
                _threadManager.ExecuteOnMainThread((() =>
                {
                    Both_Connected();
                }));
                break;
            case Commands.rdy:
                _threadManager.ExecuteOnMainThread((() =>
                {
                    Readyness(param);
                }));
                break;
            case Commands.side:
                _threadManager.ExecuteOnMainThread((() =>
                {
                    GetSide(param);
                }));
                break;
        }
    }
    /*public void RecieveMessage()
    {
        byte[] bytes = new byte[1024];
        int bytesRec = socket.Receive(bytes);
        if (bytesRec == 0) return;
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
    }*/
    private static void GetID(string[] parametrs)
    {
        client.GetId(parametrs);
        //testThread.Start();
    }

    private static void Both_Connected()
    {
        client.Both_Connected();
        //testThread.Start();
    }

    private static void Readyness(string[] parametrs)
    {
        client.Readyness(parametrs);
        //testThread.Start();
    }

    private static void GetSide(string[] parametrs)
    {
        client.GetSide(parametrs);
        //testThread.Start();
    }
}
