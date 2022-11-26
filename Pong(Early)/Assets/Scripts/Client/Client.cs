using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject gameManager;
    private Socket socket;
    public string name;
    public int id;
    [SerializeField] private Recieve recieve;
    [SerializeField] private Send send;
    [SerializeField] private GameObject loadAnim;
    [SerializeField] private GameObject connectButton;
    [SerializeField] private TMP_Text text;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetGameManager(GameObject gm)
    {
        gameManager = gm;
    }
    
    public void CreateConn()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            connectButton.SetActive(false);
            text.enabled = false;
            loadAnim.SetActive(true);
            socket.Connect("26.174.189.81", 1457);
            recieve.enabled = true;
            recieve.SetSocket(socket);
            send.SetSocket(socket);
            ListenForServer();
            Debug.Log("Connected");
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.StackTrace);
            connectButton.SetActive(true);
            text.enabled = true;
            loadAnim.SetActive(false);
        }
        
    }
    public void ExitGame()
    {
        if (socket.Connected)
        {
            socket.Close();
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

    public void WrongData()
    {
        GameObject.Find("Menu").GetComponent<MainMenu>().WrongAuth();
    }
//----------------------------------------------------------------
    
}
