using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PasekSzkolenia : MonoBehaviour
{
    public GameObject suwak;
    public float czasSzkolenia;
    public GameObject info;

    public void Szkol()
    {
        AnimacjaPaskaLadowania();
        suwak.LeanScaleX(0, 0);
        info.SetActive(false);
    }

    public void AnimacjaPaskaLadowania()
    {
        LeanTween.scaleX(suwak, 1, czasSzkolenia).setOnComplete(PokazStatus);
    }

    void PokazStatus()
    {
        info.SetActive(true);
    }
}