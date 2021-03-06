﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class OdtwarzaczOdglosow : MonoBehaviour
{
    public static AudioClip[] brzdek = new AudioClip[9];
    public static AudioClip[] smierc = new AudioClip[13];

    public static AudioSource zrodlo;
    
    void Start()
    {
        for (int i = 0; i < smierc.Length; i++)
        {
            smierc[i] = Resources.Load<AudioClip>("smierc" + i);
        }
        
        for (int i = 0; i < brzdek.Length; i++)
        {
            brzdek[i] = Resources.Load<AudioClip>("brzdek" + i);
        }

        zrodlo = GetComponent<AudioSource>();
    }

    public static void PlaySound(string odglos)
    {
        switch (odglos)
        {
            case "smierc0":
                zrodlo.PlayOneShot(smierc[0]);
                break;
            case "smierc1":
                zrodlo.PlayOneShot(smierc[1]);
                break;
            case "smierc2":
                zrodlo.PlayOneShot(smierc[2]);
                break;
            case "smierc3":
                zrodlo.PlayOneShot(smierc[3]);
                break;
            case "smierc4":
                zrodlo.PlayOneShot(smierc[4]);
                break;
            case "smierc5":
                zrodlo.PlayOneShot(smierc[5]);
                break;
            case "smierc6":
                zrodlo.PlayOneShot(smierc[6]);
                break;
            case "smierc7":
                zrodlo.PlayOneShot(smierc[7]);
                break;
            case "smierc8":
                zrodlo.PlayOneShot(smierc[8]);
                break;
            case "smierc9":
                zrodlo.PlayOneShot(smierc[9]);
                break;
            case "smierc10":
                zrodlo.PlayOneShot(smierc[10]);
                break;
            case "smierc11":
                zrodlo.PlayOneShot(smierc[11]);
                break;
            case "smierc12":
                zrodlo.PlayOneShot(smierc[12]);
                break;
            case "brzdek0":
                zrodlo.PlayOneShot(brzdek[0]);
                break;
            case "brzdek1":
                zrodlo.PlayOneShot(brzdek[1]);
                break;
            case "brzdek2":
                zrodlo.PlayOneShot(brzdek[2]);
                break;
            case "brzdek3":
                zrodlo.PlayOneShot(brzdek[3]);
                break;
            case "brzdek4":
                zrodlo.PlayOneShot(brzdek[4]);
                break;
            case "brzdek5":
                zrodlo.PlayOneShot(brzdek[5]);
                break;
            case "brzdek6":
                zrodlo.PlayOneShot(brzdek[6]);
                break;
            case "brzdek7":
                zrodlo.PlayOneShot(brzdek[7]);
                break;
            case "brzdek8":
                zrodlo.PlayOneShot(brzdek[8]);
                break;
        }
    }
}