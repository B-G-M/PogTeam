using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Client : MonoBehaviour
{
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private GameObject gameManager;
    private Socket socket;
    public string name;
    public int id;
    private string _ip;
    [SerializeField] private Recieve recieve;
    [SerializeField] private Send send;
    [SerializeField] private GameObject loadAnim;
    [SerializeField] private GameObject connectButton;
    [SerializeField] private GameObject registrationBtn;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject findingServer;
    private Thread findServer = null;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        findingServer.SetActive(true);
        findServer = new Thread(new ThreadStart(AutoDiscoverServer));
        findServer.Start();
    }

    public void SetGameManager(GameObject gm)
    {
        gameManager = gm;
    }

    public void AutoDiscoverServer()
    {
        bool received = false;
        int PORT = 2010;
        UdpClient udpClient = new UdpClient();
        udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));
        udpClient.EnableBroadcast = true;
        var from = new IPEndPoint(0, 0);
        string myMess = "";
        while (!received)
        {
            var recvBuffer = udpClient.Receive(ref from);
            string mess = Encoding.UTF8.GetString(recvBuffer);
            if (mess == "LetsPlayPong") received = true;
        }

        _ip = from.ToString();
        findServer.Abort();
        findingServer.SetActive(false);
    }
    
    public void CreateConn(string login, string password, string typeOfEntrance)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            connectButton.SetActive(false);
            registrationBtn.SetActive(false);
            text.enabled = false;
            loadAnim.SetActive(true);
            socket.Connect("192.168.1.8", 2009);
            recieve.enabled = true;
            recieve.SetSocket(socket);
            send.SetSocket(socket);
            ListenForServer();
            Debug.Log("Connected");
            switch (typeOfEntrance)
            {
                case "auth":
                    Authirization(login, password);
                    break;
                case "rgstr":
                    Registration(login, password);
                    break;
            }
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.StackTrace);
            connectButton.SetActive(true);
            registrationBtn.SetActive(true);
            text.enabled = true;
            loadAnim.SetActive(false);
            warningText.enabled = true;
            warningText.text = "Cannot connect to the server";
        }
    }
    public void ExitGame()
    {
        if (socket.Connected)
        {
            socket.Close();
            SceneManager.LoadScene(0);
        }
    }

    private void ListenForServer()
    {
        recieve.StartListening();  
    }
//--------------------Send requests-----------------------------
    public void Authirization(string login, string password)
    {
        name = login;
        send.GetAuthentification(login, password);
    }

    public void Registration(string login, string password)
    {
        name = login;
        send.Registration(login, password);
    }
    public void SendReadyness(bool isReady)
    {
        send.SendReadyness(id, isReady);
    }

    public void RequestSide()
    {
        send.SendSide();
    }

    public void SendSatus(string status)
    {
        send.SendStatus(status);
    }
    public void ResendLastCommand()
    {
        send.ResendLastCommand();
    }

    public void MoveUp()
    {
        send.Send_Command_On_Move_Up();
    }

    public void MoveDown()
    {
        send.Send_Command_On_Move_Down();
    }

    public void GameIsStart()
    {
        send.SendGameIsStart();
    }

    public void LeaderList()
    {
        //send.LeaderList();
    }

    public void BallHasReachedCollider()
    {
        send.BallHasReachedCollider();
    }
//---------------------------------------------------------------    


//-------------------Get Requests--------------------------------
    public void GetId(string[] parametrs)
    {
        id = Convert.ToInt32(parametrs[1]);
        SceneManager.LoadScene(1);
        Debug.Log($"ID: {parametrs[1]}");
    }

    public void GetSide(string[] parametrs)
    {
        gameManager.GetComponent<GameManager_Script>().SetSide(Convert.ToInt32(parametrs[1]), parametrs[2]);
        Debug.Log("My side: "+parametrs[1]);
    }
    public void Readyness(string[] parametrs)
    {
        gameManager.GetComponent<GameManager_Script>().SetOtherPlayerStatus(Convert.ToBoolean(parametrs[1]));
    }
    public void Accept_Ready()
    {
        gameManager.GetComponent<GameManager_Script>().Accept_Ready();
    }

    public void Accept_NotReady()
    {
        gameManager.GetComponent<GameManager_Script>().Accept_NotReady();
    }

    public void Accept_Enemy_Ready()
    {
        gameManager.GetComponent<GameManager_Script>().Accept_Enemy_Ready();
    }

    public void Accept_Enemy_NotReady()
    {
        gameManager.GetComponent<GameManager_Script>().Accept_Enemy_NotReady();
    }

    public void Move_Up(float position, float dir, float speed)
    {
        gameManager.GetComponent<GameManager_Script>().Accept_Request_For_Move_Up(position, dir, speed);
    }

    public void Move_Down(float position, float target, float speed)
    {
        gameManager.GetComponent<GameManager_Script>().Accept_Request_For_Move_Down(position, target, speed);
    }

    public void MoveEnemy(float position, float target, float speed)
    {
        gameManager.GetComponent<GameManager_Script>().Accept_Request_For_Move_Enemy(position, target, speed);
    }

    public void SetBallPos(float x, float y)
    {
        gameManager.GetComponent<GameManager_Script>().SetBallPos(x, y);
    }
    public void WrongData()
    {
        connectButton.SetActive(true);
        registrationBtn.SetActive(true);
        GameObject.Find("Menu").GetComponent<MainMenu>().WrongAuth();
    }

    public void FormLeaderList(string[] arguments)
    {
        gameManager.GetComponent<GameManager_Script>().FormLeaderList(arguments);
    }

    public void Scored(string[] arguments)
    {
        gameManager.GetComponent<GameManager_Script>().Scored(arguments);
    }

    public void GotScored(string[] arguments)
    {
        gameManager.GetComponent<GameManager_Script>().GotScored(arguments);
    }

    public void ChangeBallDirection(string[] arguments)
    {
        gameManager.GetComponent<GameManager_Script>().ChangeBallDirection(arguments);
    }

    public void ReceiveEndGame(string message)
    {
        gameManager.GetComponent<GameManager_Script>().ReceiveEndGameMessage(message);
    }
//----------------------------------------------------------------
    
}
