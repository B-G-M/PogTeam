using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCoundEnd : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;

    public void StartGame()
    {
        gameManager.GetComponent<GameManager_Script>().StartGame();
    }
}
