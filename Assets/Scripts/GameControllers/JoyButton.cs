﻿using UnityEngine;
using UnityEngine.EventSystems;

public class JoyButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool pressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}
