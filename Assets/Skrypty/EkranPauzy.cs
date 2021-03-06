﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EkranPauzy : MonoBehaviour
{
    [SerializeField]
    GameObject panelPrzerwy = null;

    private bool statusPauzy = false;

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (statusPauzy)
            {
                Time.timeScale = 1;
                statusPauzy = false;
                panelPrzerwy.SetActive(false);

            }
            else
            {
                Time.timeScale = 0;
                statusPauzy = true;
                panelPrzerwy.SetActive(true);
            }
        }
    }
}