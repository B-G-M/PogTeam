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
        rdy,
        side,
        rdy_ok,
        error
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

    private void switchFunc(string[] arguments)
    {
        switch ((Commands)Enum.Parse(typeof(Commands), arguments[0]))
        {
            case Commands.id:
                _threadManager.ExecuteOnMainThread(() => { GetID(arguments); });
                break;
            case Commands.rdy:
                _threadManager.ExecuteOnMainThread((() => { Readyness(arguments); }));
                break;
            case Commands.side:
                _threadManager.ExecuteOnMainThread((() => { GetSide(arguments); }));
                break;
            case Commands.rdy_ok:
                _threadManager.ExecuteOnMainThread(() => { AcceptReadyness(); });
                break;
            case Commands.error:
                _threadManager.ExecuteOnMainThread(() => { ReSendLastCommand(); });
                break;
            default:
                SendStatus($"error_{arguments[0]}");
                break;
        }
    }
    private void ThreadAction() 
    {
        while (true)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            String res = Encoding.UTF8.GetString(bytes, 0, bytesRec);
            String[] commands = res.Split(";");
            for (int i = 0; i < commands.Length - 1; i++)
            {
                String[] param = commands[i].Split("_");
                switchFunc(param);
            }
        }
    }
    private static void GetID(string[] parametrs)
    {
        client.GetId(parametrs);
        //testThread.Start();
    }
    
    private static void Readyness(string[] parametrs)
    {
        client.Readyness(parametrs);
        //testThread.Start();
    }

    private static void GetSide(string[] parametrs)
    {
        Debug.Log("message: "+parametrs[0]+parametrs[1]);
        client.GetSide(parametrs);
        //testThread.Start();
    }

    private static void AcceptReadyness()
    {
        client.AcceptReadyness();
    }

    private static void ReSendLastCommand()
    {
        client.ResendLastCommand();
    }

    private static void SendStatus(string status)
    {
        client.SendSatus(status);
    }
}
