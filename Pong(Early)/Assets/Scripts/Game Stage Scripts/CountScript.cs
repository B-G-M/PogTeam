using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountScript : MonoBehaviour
{
    [SerializeField] private TMP_Text right;
    [SerializeField] private TMP_Text left;
    private int _rightCount;
    private int _leftCount;

    private void Update()
    {
        right.text = _rightCount.ToString();
        left.text = _leftCount.ToString();
    }

    private void Awake()
    {
        _rightCount = 0;
        _leftCount = 0;
    }

    public void AddRight() => _rightCount++;
    public void AddLeft() => _leftCount++;
}
