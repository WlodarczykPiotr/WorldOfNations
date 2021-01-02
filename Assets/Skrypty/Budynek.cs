using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Budynek : MonoBehaviour,InterfejsWyboru
{
    [SerializeField]
    Transform punktUtworzenia = null, choragiew = null;
    [SerializeField]
    GameObject pasekZycia = null;

    protected PasekZycia pasek_zycia;

    void Start()
    {
        Jednostka.ZaznaczoneJednostki.Add(this);
        pasek_zycia = Instantiate(pasekZycia, transform).GetComponent<PasekZycia>();
        UstawZaznaczenie(false);
    }

    public void UstawZaznaczenie(bool zaznaczenie)
    {
        choragiew.gameObject.SetActive(zaznaczenie);
        pasek_zycia.gameObject.SetActive(zaznaczenie);
    }
 
    void UtworzJednostke(GameObject rycerz)
    {
        Transakcja transakcja = rycerz.GetComponent<Transakcja>();

        if(!transakcja || !Surowce.UjmijZywnosc(transakcja.zywnosc))
        {
            return;
        }
        else if(!transakcja || !Surowce.UjmijDrewno(transakcja.drewno))
        {
            return;
        }
        else if(!transakcja || !Surowce.UjmijKamien(transakcja.kamien))
        {
            return;
        }
        else if(!transakcja || !Surowce.UjmijZloto(transakcja.zloto))
        {
            return;
        }

        GameObject jednostka = Instantiate(rycerz, punktUtworzenia.position, punktUtworzenia.rotation);

        jednostka.SendMessage("Polecenie", choragiew.position, SendMessageOptions.DontRequireReceiver);

        PladrowanieZwlok.PokazLupy(jednostka.transform.position, -transakcja.zywnosc, -transakcja.drewno, -transakcja.kamien, -transakcja.zloto);
    }

    void Polecenie(Jednostka jednostka)
    {
        Polecenie(jednostka.transform.position);
    }

    void Polecenie(Vector3 pozycjaChoragwi)
    {
        choragiew.position = pozycjaChoragwi;
    }

    private void OnDestroy()
    {
        Jednostka.ZaznaczoneJednostki.Remove(this);
    }
}