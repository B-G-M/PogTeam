using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    [SerializeField] private TMP_Text message;

    public void ShowMessage(bool status)
    {
        message.text = status ? "You've won!" : "You've lost...";
    }
}
