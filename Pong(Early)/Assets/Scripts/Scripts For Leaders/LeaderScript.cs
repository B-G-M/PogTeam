using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderScript : MonoBehaviour
{
    [SerializeField] private TMP_Text nickname;
    [SerializeField] private TMP_Text wins;

    public void SetInfo(string _nickname, int _wins )
    {
        nickname.text = _nickname;
        wins.text = _wins.ToString();
    }
}
