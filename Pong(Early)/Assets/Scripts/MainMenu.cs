using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject client;
    public void Play()
    {
        client.GetComponent<Client>().CreateConn();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
