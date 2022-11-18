using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using Unity.VisualScripting;

public class Recieve : MonoBehaviour
{
    [SerializeField] private static Client client;
    private static object sync = new object();
    public static Queue <Action> queue = new Queue<Action>();
    private static Socket socket;
    private Thread testThread = null;
    private enum Commands
    {
        id,
        both_con,
        rdy,
        side
    }


    public void Update()
    {
        lock (sync)
        {
            foreach (var action in queue)
            {
                action.Invoke();
                queue.Dequeue();
            }
        }
    }
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }
    public static void Execute(Action action){
        lock(sync){ 
            queue.Enqueue(action);
        }
        Thread.Sleep(1000);
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
                Execute(() =>
                {
                    GetID(param);
                    testThread.Interrupt();
                });
                break;
            case Commands.both_con:
                Execute(() =>
                {
                    Both_Connected();
                    testThread.Interrupt();
                });
                break;
            case Commands.rdy:
                Execute(() =>
                {
                    Readyness(param);
                    testThread.Interrupt();
                });
                break;
            case Commands.side:
                Execute(() =>
                {
                    GetSide(param);
                    testThread.Interrupt();
                });
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
    }

    private static void Both_Connected()
    {
        client.Both_Connected();
    }

    private static void Readyness(string[] parametrs)
    {
        client.Readyness(parametrs);
    }

    private static void GetSide(string[] parametrs)
    {
        client.GetSide(parametrs);
    }
}
