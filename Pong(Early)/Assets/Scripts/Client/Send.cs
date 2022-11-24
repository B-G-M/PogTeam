using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Send : MonoBehaviour
{
    private Socket socket;
    private static readonly Queue<Action> lastExecutedCommand = new Queue<Action>();
    
    public void SetSocket(Socket _socket)
    {
        socket = _socket;
    }
    private void EnqueueCommand(Action _action)
    {
        if (_action == null)
        {
            Debug.Log("No action to execute");
            return;
        }
        lastExecutedCommand.Enqueue(_action);
    }
    public void GetAuthentification(String login, String password)
    {
        try
        {
            lastExecutedCommand.Dequeue();
        }
        catch (Exception ex)
        {
            Debug.Log($"Queue is empry: {ex}");
        }
        
        String message = $"auth_{login}_{password};";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
        EnqueueCommand(() => { GetAuthentification(login, password); });
    }

    public void SendSide()
    {
        try
        {
            lastExecutedCommand.Dequeue();
        }
        catch (Exception ex)
        {
            Debug.Log($"Queue is empry: {ex}");
        }
        
        string message = $"chSide;";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
        EnqueueCommand(() => { SendSide(); });
    }

    public void SendReadyness(int id, bool ready)
    {
        try
        {
            lastExecutedCommand.Dequeue();
        }
        catch (Exception ex)
        {
            Debug.Log($"Queue is empry: {ex}");
        }
        
        string message = $"rdy_{ready};";
        byte[] requestData = Encoding.UTF8.GetBytes(message);
        socket.Send(requestData);
        EnqueueCommand(() => { SendReadyness(id, ready); });
    }

    // public void SendError()
    // {
    //     string message = "error";
    //     byte[] requestData = Encoding.UTF8.GetBytes(message);
    //     socket.Send(requestData);
    // }
    public void ResendLastCommand()
    {
        lastExecutedCommand.Dequeue().Invoke();
    }

    public void SendStatus(string status)
    {
        byte[] requestData = Encoding.UTF8.GetBytes(status);
        socket.Send(requestData);
    }
}
