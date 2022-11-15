using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;

public class ChooseSide_Script : MonoBehaviour
{
    [SerializeField] private GameObject rightBtn;
    [SerializeField] private GameObject leftBtn;
    [SerializeField] private GameObject rightPlayer;
    [SerializeField] private GameObject leftPlayer;

    public void ClickRightBtn()
    {
        rightPlayer.GetComponent<Player_Script>().enabled = true;
        leftPlayer.GetComponent<Player_Script>().enabled = false;
        rightBtn.GetComponent<SpriteRenderer>().color = Color.white;
        leftBtn.GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void ClickLeftBtn()
    {
        leftPlayer.GetComponent<Player_Script>().enabled = true;
        rightPlayer.GetComponent<Player_Script>().enabled = false;
        rightBtn.GetComponent<SpriteRenderer>().color = Color.black;
        leftBtn.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
