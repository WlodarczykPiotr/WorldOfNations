using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class OdtwarzaczMuzyki : MonoBehaviour
{
    [SerializeField]
    AudioClip[] utwory = null;

    AudioSource zrodlo;

    void Start()
    {
        zrodlo = FindObjectOfType<AudioSource>();
        zrodlo.loop = false;
    }

    AudioClip WczytajUtwor()
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