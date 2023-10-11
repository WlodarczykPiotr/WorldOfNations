using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class Komputer : Jednostka
{
    [SerializeField]
    float promienZasiegu = 5;
    [SerializeField]
    float czasPatrolowania = 2;
    [SerializeField]
    float szybkoscGonitwy = 5;
    [SerializeField]
    ushort zywnosc = 100, drewno = 60, kamien = 40, zloto = 20;

    float normalnaSzybkosc;

    float czasSpoczynku = 1;

    Vector3 pozycjaStartowa;

    List<Czlowiek> listaJednostekGracza = new List<Czlowiek>();

    Czlowiek NajblizszaJednostka
    {
        get
        {
            if (listaJednostekGracza == null || listaJednostekGracza.Count <= 0)
            {
                return null;
            }

            float najmniejszaOdleglosc = float.MaxValue;

            Czlowiek najblizszaJednostka = null;

            foreach (Czlowiek czlowiek in listaJednostekGracza)
            {
                if (!czlowiek || !czlowiek.CzyZyje)
                {
                    continue;
                }

                float odleglosc = Vector3.Magnitude(czlowiek.transform.position - transform.position);

                if (odleglosc < najmniejszaOdleglosc)
                {
                    najmniejszaOdleglosc = odleglosc;
                    najblizszaJednostka = czlowiek;
                }
            }

            return najblizszaJednostka;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        normalnaSzybkosc = nawigacja.speed;

        pozycjaStartowa = transform.position;
    }

    protected override void Start()
    {
        base.Start();
        KontrolerGry.ListaJednostekPrzeciwnika.Add(this);
    }

    protected override void Spocznij()
    {
        base.Spocznij();

        ZaktualizujWidok();

        if ((czasSpoczynku -= Time.deltaTime) <= 0)
        {
            czasSpoczynku = czasPatrolowania;

            polecenie = Polecenie.idz;

            UstawDowolnaPozycjeWedrowania();
        }
    }

    protected override void Idz()
    {
        base.Idz();

        nawigacja.speed = normalnaSzybkosc;

        ZaktualizujWidok();
    }

    protected override void Gon()
    {
        base.Gon();

        nawigacja.speed = szybkoscGonitwy;
    }

    void UstawDowolnaPozycjeWedrowania()
    {
        Vector3 przesuniecie = new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f));

        przesuniecie.Normalize();
        przesuniecie *= promienZasiegu;
        
        nawigacja.SetDestination(pozycjaStartowa + przesuniecie);
    }

    protected override void PrzyjmijObrazenia(float obrazenia, Vector3 pozycjaZadawaniaObrazen)
    {
        base.PrzyjmijObrazenia(obrazenia, pozycjaZadawaniaObrazen);

        if (!cel && CzyZyje)
        {
            polecenie = Polecenie.idz;
            nawigacja.SetDestination(pozycjaZadawaniaObrazen);
        }

        if (PasekZakres < 1f)
        {
            nawigacja.velocity = Vector3.zero;
        }

        if (!CzyZyje && Surowce.DodajZywnosc(zywnosc) && Surowce.DodajDrewno(drewno) && Surowce.DodajKamien(kamien) && Surowce.DodajZloto(zloto))
        {
            PladrowanieZwlok.PokazLupy(transform.position, zywnosc, drewno, kamien, zloto);
        }    
    }

    
    void OnTriggerEnter(Collider kolizja)
    {
        Czlowiek czlowiek = kolizja.gameObject.GetComponent<Czlowiek>();

        if (czlowiek && !listaJednostekGracza.Contains(czlowiek))
        {
            listaJednostekGracza.Add(czlowiek);
        }
    }

   void OnTriggerExit(Collider kolizja)
    {
        Czlowiek czlowiek = kolizja.gameObject.GetComponent<Czlowiek>();

        if (czlowiek && listaJednostekGracza.Contains(czlowiek))
        {
            listaJednostekGracza.Remove(czlowiek);
        }
    }

    void ZaktualizujWidok()
    {
        Czlowiek czlowiek = NajblizszaJednostka;

        if (czlowiek)
        {
            cel = czlowiek.transform;
            polecenie = Polecenie.gon;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;

        if (!Application.isPlaying)
        {
            pozycjaStartowa = transform.position;
        }

        Gizmos.DrawWireSphere(pozycjaStartowa, promienZasiegu);
    }
}