using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitintMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject rightCircle;
    [SerializeField] private GameObject leftCircle;
    [SerializeField] private CanvasGroup _canvasGroup;

    // Update is called once per frame
    void Update()
    {
        if (_canvasGroup.alpha == 0f)
        {
            rightCircle.SetActive(false);
            leftCircle.SetActive(false);
        }
        else
        {
            rightCircle.SetActive(true);
            leftCircle.SetActive(true);
        }
    }
}
