﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    #region Fields

    public float fadeSpeed = 2f;
    public float highIntensity = 2f;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;
    public bool alarmOn;

    private float targetIntensity;

    new Light light;
    
    #endregion

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Start()
    {
        light.intensity = 0;
        targetIntensity = highIntensity;
    }

    private void Update()
    {
        if (alarmOn)
        {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            CheckTargetIntensity();
        }
        else
        {
            light.intensity = Mathf.Lerp(light.intensity, 0f, fadeSpeed * Time.deltaTime);
        }
    }

    #region Control alarm

    private void CheckTargetIntensity()
    {
        if (Mathf.Abs(targetIntensity - light.intensity) < changeMargin)
        {
            if (targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }
            else
            {
                targetIntensity = highIntensity;
            }
        }
    }

    #endregion
}
