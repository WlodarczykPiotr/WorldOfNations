using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdtwarzaczMuzyki : MonoBehaviour
{
    public AudioClip[] utwory;

    private AudioSource zrodlo;

    void Start()
    {
        zrodlo = FindObjectOfType<AudioSource>();
        zrodlo.loop = false;
    }

    private AudioClip WczytajUtwor()
    {
        return utwory[Random.Range(0, utwory.Length)];
    }

    void Update()
    {
        if(!zrodlo.isPlaying)
        {
            zrodlo.clip = WczytajUtwor();
            zrodlo.Play();
        }
    }
}
