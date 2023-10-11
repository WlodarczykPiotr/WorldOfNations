using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PrzyciskJednostki : MonoBehaviour
{
    [SerializeField]
    protected GameObject prefabrykat = null;

    Text tekst;
    Button przycisk;

    void Awake()
    {
        tekst = GetComponentInChildren<Text>(true);
        przycisk = GetComponentInChildren<Button>(true);

        Transakcja transakcja;

        if (prefabrykat && (transakcja = prefabrykat.GetComponent<Transakcja>()))
        {
            przycisk.image.sprite = transakcja.zdjecie;
        }
    }

    void Update()
    {
        Transakcja transakcja;

        if (prefabrykat && (transakcja = prefabrykat.GetComponent<Transakcja>()))
        {
            tekst.text = "Zywność: " + transakcja.zywnosc + " Drewno: " + transakcja.drewno + " Kamień: " + transakcja.kamien + " Złoto: " + transakcja.zloto;
            przycisk.interactable = Surowce.CzyStac(transakcja.zywnosc, transakcja.drewno, transakcja.kamien, transakcja.zloto);
        }
    }

    protected virtual void UtworzJednostke()
    {
        StartCoroutine(Szkolenie());
    }

    private IEnumerator Szkolenie()
    {
        yield return new WaitForSeconds(1.0f);
        KontrolerKamery.UtworzJednostki(prefabrykat);
    }
}