using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Btn_Script : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float minimum ;
    public float maximum ;
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        transform.localScale = new Vector2(Mathf.Lerp(minimum, maximum, Time.time), Mathf.Lerp(minimum, maximum, Time.time));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        transform.localScale = new Vector2(Mathf.Lerp(maximum, minimum, Time.time), Mathf.Lerp(maximum, minimum, Time.time));
    }
}
