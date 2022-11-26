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
        Ready,
        NotReady,
        changeRdy_Ready,
        changeRdy_NotReady,
        changeRdy,
        moveUp,
        moveDown,
        auth,
        sMoveUp,
        sMoveDown
    }

    private enum Status
    {
        Ok,
        Error
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
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread(() => { GetID(arguments); });
                break;
            case Commands.auth:
                switch ((Status)Enum.Parse(typeof(Status), arguments[1]))
                {
                    case Status.Error:
                        _threadManager.ExecuteOnMainThread(() => { WrongAuth(); });
                        break;
                }
                break;
            case Commands.rdy:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread((() => { Readyness(arguments); }));
                break;
            case Commands.side:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread((() => { GetSide(arguments); }));
                break;
            case Commands.Ready:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread(() => { Accept_Ready(); });
                break;
            case Commands.NotReady:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread(() => { Accept_NotReady(); });
                break;
            case Commands.changeRdy_Ready:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread(() => { Accept_Enemy_Ready(); });
                break;
            case Commands.changeRdy_NotReady:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                _threadManager.ExecuteOnMainThread(() => { Accept_Enemy_NotReady(); });
                break;
            case Commands.changeRdy:
                if (arguments.Length > 1 && arguments[1] == "ERROR")
                {
                    Debug.Log(arguments);
                    break;
                }
                switch (arguments[1])
                {
                    case "Ready":
                        _threadManager.ExecuteOnMainThread(() => { Accept_Enemy_Ready(); });
                        break;
                    case "NotReady":
                        _threadManager.ExecuteOnMainThread(() => { Accept_Enemy_NotReady(); });
                        break;
                }
                break;
            case Commands.moveUp:
                _threadManager.ExecuteOnMainThread(() => { Move_Up(arguments); });
                break;
            case Commands.moveDown:
                _threadManager.ExecuteOnMainThread(() => { Move_Down(arguments); });
                break;
            case Commands.sMoveUp:
                _threadManager.ExecuteOnMainThread(()=>{MoveEnemy(arguments);});
                break;
            case Commands.sMoveDown:
                _threadManager.ExecuteOnMainThread(()=>{MoveEnemy(arguments);});
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
    private static void Accept_Ready()
    {
        client.Accept_Ready();
    }

    private static void Accept_NotReady()
    {
        client.Accept_NotReady();
    }

    private static void Accept_Enemy_Ready()
    {
        client.Accept_Enemy_Ready();
    }

    private static void Accept_Enemy_NotReady()
    {
        client.Accept_Enemy_NotReady();
    }

    private static void ReSendLastCommand()
    {
        client.ResendLastCommand();
    }

    private static void SendStatus(string status)
    {
        client.SendSatus(status);
    }

    private static void Move_Up(string[] param)
    {
        client.Move_Up(Convert.ToSingle(param[1]), Convert.ToSingle(param[2]), Convert.ToSingle(param[3]));
    }

    private static void Move_Down(string[] param)
    {
        client.Move_Down(Convert.ToSingle(param[1]), Convert.ToSingle(param[2]), Convert.ToSingle(param[3]));
    }

    private static void MoveEnemy(string[] param)
    {
        client.MoveEnemy(Convert.ToSingle(param[1]), Convert.ToSingle(param[2]), Convert.ToSingle(param[3]));
    }
    

    private static void WrongAuth()
    {
        client.WrongData();
    }
    
}
