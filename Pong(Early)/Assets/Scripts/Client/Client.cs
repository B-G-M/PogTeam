using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
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
    [SerializeField] private Button connectButton;
    [SerializeField] private TMP_Text text;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public  Socket CreateConn()
    {
        var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            client.Connect("10.102.228.103", 1457);
            recieve.enabled = true;
            recieve.SetSocket(client);
            send.SetSocket(client);
            ListenForServer();
            Debug.Log("Connected");
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.StackTrace);
            connectButton.enabled = true;
            text.enabled = true;
            loadAnim.SetActive(false);
        }
        return client;
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
        login = "fmfmf";
        password = "jnvjn";
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
        gameManager.GetComponent<GameManager_Script>().SetSide(Convert.ToInt32(parametrs[1]));
    }

    public void Both_Connected()
    {
        gameManager.GetComponent<GameManager_Script>().BothConnected();
    }

    public void Readyness(string[] parametrs)
    {
        gameManager.GetComponent<GameManager_Script>().SetOtherPlayerStatus(Convert.ToBoolean(parametrs[1]));
    }

    public void AcceptReadyness()
    {
        gameManager.GetComponent<GameManager_Script>().Acceptreadyness();
    }
//----------------------------------------------------------------
    
}
