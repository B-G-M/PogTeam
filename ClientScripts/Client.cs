using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Client : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private Socket socket;
    public string name;
    public int id;
    [SerializeField] private Recieve_Send_scrpt recieve_send;
    [SerializeField] private Commands command;
    private void Awake()
    {
        Socket clientConn = CreateConn();
        recieve_send.SetSocket(clientConn);
    }

    private void Update()
    {
        recieve_send.RecieveMessage();
    }

    private Socket CreateConn()
    {
        var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            client.Connect("10.162.250.246", 8080);
            Debug.Log("Connected");
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.StackTrace);
        }
        return client;
    }

    public void Authirization(string login, string password)
    {
        command.GetAuthentification(login, password);
    }

    public void ExitGame()
    {
        command.ExitGame();
    }

    public void SendSide(int side)
    {
        command.SendSide(id, side);
    }

    public void GetId(string[] parametrs)
    {
        
    }

    public void Both_Connected()
    {
        
    }

    public void Readyness(string[] parametrs)
    {
        
    }

    
}
