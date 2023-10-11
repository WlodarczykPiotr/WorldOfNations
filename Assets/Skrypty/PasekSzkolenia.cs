using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class PasekSzkolenia : MonoBehaviour
{
    [SerializeField]
    GameObject suwak = null;
    [SerializeField]
    float czasSzkolenia = 0;
    [SerializeField]
    GameObject info = null;

    void Szkol()
    {
        AnimacjaPaskaLadowania();
        suwak.LeanScaleX(0, 0);
        info.SetActive(false);
    }

    void AnimacjaPaskaLadowania()
    {
        LeanTween.scaleX(suwak, 1, czasSzkolenia).setOnComplete(PokazStatus);
    }

    void PokazStatus()
    {
        info.SetActive(true);
    }
}