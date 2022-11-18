using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject authirizationField;
    [SerializeField] private GameObject loadAnim;
    [SerializeField] private Button connectButton;
    [SerializeField] private TMP_InputField login;
    [SerializeField] private TMP_InputField password;
    public void Play()
    {
        authirizationField.SetActive(true);
    }

    public void Connect()
    {
        /*if (login.text == null || password.text == null)
        {
            Debug.Log("login or password are not valid");
            return;
        }*/
        loadAnim.SetActive(true);
        connectButton.enabled = false;
        client.GetComponent<Client>().CreateConn();
        client.GetComponent<Client>().Authirization(login.text, password.text);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
