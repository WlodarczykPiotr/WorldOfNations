using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrzyciskJednostki : MonoBehaviour
{
    [SerializeField]
    protected GameObject prefabrykat;

    Text tekst;
    Button przycisk;

    private void Awake()
    {
        tekst = GetComponentInChildren<Text>(true);
        przycisk = GetComponentInChildren<Button>(true);

        Transakcja transakcja;

        if (prefabrykat && (transakcja = prefabrykat.GetComponent<Transakcja>()))
        {
            przycisk.image.sprite = transakcja.zdjecie;
        }
    }

    private void Update()
    {
        Transakcja transakcja;

        if (prefabrykat && (transakcja = prefabrykat.GetComponent<Transakcja>()))
        {
            tekst.text = "Zywność: " + transakcja.zywnosc + " Drewno: " + transakcja.drewno + " Kamień: " + transakcja.kamien + " Złoto: " + transakcja.zloto;
            przycisk.interactable = Surowce.CzyStac(transakcja.zywnosc, transakcja.drewno, transakcja.kamien, transakcja.zloto);
        }
    }

    public virtual void UtworzJednostke()
    {
        StartCoroutine(Szkolenie());
    }

    public IEnumerator Szkolenie()
    {
        yield return new WaitForSeconds(5.0f);
        KontrolerKamery.UtworzJednostki(prefabrykat);
    }
}