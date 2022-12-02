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
    [SerializeField] private TMP_Text warningText;
    public void Play()
    {
        authirizationField.SetActive(true);
    }

    public void CloseWindows()
    {
        authirizationField.SetActive(false);
    }

    public void Connect()
    {
        warningText.enabled = true;
        if (login.text == "" || password.text == "")
        {
            warningText.text = "login or password are not valid";
            return;
        }
        loadAnim.SetActive(true);
        connectButton.enabled = false;
        client.GetComponent<Client>().CreateConn();
        //client.GetComponent<Client>().AutoDiscoverServer();
        client.GetComponent<Client>().Authirization(login.text, password.text);
        warningText.enabled = false;
    }

    public void Registration()
    {
        warningText.enabled = true;
        if (login.text == "" || password.text == "")
        {
            warningText.text = "login or password are not valid";
            return;
        }
        loadAnim.SetActive(true);
        client.GetComponent<Client>().CreateConn();
        client.GetComponent<Client>().Registration(login.text, GFGEncryption.encodeString(password.text));
        warningText.enabled = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void WrongAuth()
    {
        loadAnim.SetActive(false);
        connectButton.enabled = true;
        warningText.enabled = false;
        
    }
}
