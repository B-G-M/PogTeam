using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ReadyBtn_Script : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Sprite checkMark;
    [SerializeField] private Sprite cross;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject gameManager;
    private bool isLeftReady = false;
    private bool isRightReady = false;
    
    public void LeftBtn()
    {
        if (!isLeftReady)
        {
            image.GetComponent<Image>().sprite = checkMark;
            text.text = "Not ready";
            gameManager.GetComponent<GameManager_Script>().LeftPlayerStatus();
            isLeftReady = true;
            
        }
        else
        {
            image.GetComponent<Image>().sprite = cross;
            text.text = "ready";
            gameManager.GetComponent<GameManager_Script>().LeftPlayerStatus();
            isLeftReady = false;
        }
    }

    public void RightBtn()
    {
        if (!isRightReady)
        {
            image.GetComponent<Image>().sprite = checkMark;
            text.text = "Not ready";
            gameManager.GetComponent<GameManager_Script>().RightPlayerStatus();
            isRightReady = true;
        }
        else
        {
            image.GetComponent<Image>().sprite = cross;
            text.text = "ready";
            gameManager.GetComponent<GameManager_Script>().RightPlayerStatus();
            isRightReady = false;
        }
    }
}
