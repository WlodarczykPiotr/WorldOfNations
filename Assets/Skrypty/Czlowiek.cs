using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Czlowiek : Jednostka, InterfejsWyboru
{
    [Header("Czlowiek")]
    [SerializeField]
    ParticleSystem efektCiosu = null, efektUderzenia = null;
    [SerializeField]
    LayerMask warstwaCiosu = 0;
    [Range(0, .3f), SerializeField]
    float dlugoscAtaku = 1;
    [SerializeField]
    float szybkoscGonitwy = 4;

    float normalnaSzybkosc = 3;

    Light efektSwiatla;

    protected override void Awake()
    {
        base.Awake();

        efektSwiatla = efektCiosu.GetComponent<Light>();

        efektUderzenia.transform.SetParent(null);

        KoniecCiosu();

        enabled = true;
    }

    protected override void Start()
    {
        base.Start();
        KontrolerGry.ListaJednostekGracza.Add(this);
    }

    public void UstawZaznaczenie(bool zaznaczenie)
    {
        pasek_zycia.gameObject.SetActive(zaznaczenie);
    }

    new void Polecenie(Vector3 podazanie)
    {
        if (CzyZyje)
        {
            nawigacja.SetDestination(podazanie);
            polecenie = Jednostka.Polecenie.idz;
            cel = null;
        }
    }

    new void Polecenie(Czlowiek sledzenie)
    {
        cel = sledzenie.transform;
        polecenie = Jednostka.Polecenie.sledz;
    }

    new void Polecenie(Komputer Cel)
    {
        cel = Cel.transform;
        polecenie = Jednostka.Polecenie.gon;
    }

    protected override void Spocznij()
    {
        base.Spocznij();

        ZaktualizujWidok();
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

    public override void ZadajObrazenia()
    {
        if (Cios())
        {
            base.ZadajObrazenia();
        }
    }

    bool Cios()
    {
        Vector3 start = efektCiosu.transform.position;
        Vector3 kierunek = transform.forward;

        RaycastHit uderzenie;

        if (Physics.Raycast(start, kierunek, out uderzenie, odlegloscDoAtaku, warstwaCiosu))
        {
            PoczatekCiosu(start, uderzenie.point, true);
            Jednostka jednostka = uderzenie.collider.gameObject.GetComponent<Jednostka>();

            if (jednostka)
            {
                return true;
            }
        }

        PoczatekCiosu(start, start + kierunek * odlegloscDoAtaku, false);

        return false;
    }

    void PoczatekCiosu(Vector3 poczatekCelu, Vector3 koniecCelu, bool uderzenieCelu)
    {
        if (uderzenieCelu)
        {
            efektUderzenia.transform.position = koniecCelu;
            efektUderzenia.Play();
        }

        efektSwiatla.enabled = true;

        efektCiosu.Play();

        Invoke("KoniecCiosu", dlugoscAtaku);
    }

    void KoniecCiosu()
    {
        efektSwiatla.enabled = false;
    }

    public override void PrzyjmijObrazenia(float obrazenia, Vector3 pozycjaZadawaniaObrazen)
    {
        base.PrzyjmijObrazenia(obrazenia, pozycjaZadawaniaObrazen);

        //animator.SetTrigger("Cios");
    }

    List<Komputer> listaJednostekPrzeciwnika = new List<Komputer>();

    Komputer NajblizszaJednostka
    {
        get
        {
            if (listaJednostekPrzeciwnika == null || listaJednostekPrzeciwnika.Count <= 0)
            {
                return null;
            }

            float najmniejszaOdleglosc = float.MaxValue;

            Komputer najblizszaJednostka = null;

            foreach (Komputer komputer in listaJednostekPrzeciwnika)
            {
                if (!komputer || !komputer.CzyZyje)
                {
                    continue;
                }

                float odleglosc = Vector3.Magnitude(komputer.transform.position - transform.position);

                if (odleglosc < najmniejszaOdleglosc)
                {
                    najmniejszaOdleglosc = odleglosc;
                    najblizszaJednostka = komputer;
                }
            }

            return najblizszaJednostka;
        }
    }

    private void OnTriggerEnter(Collider kolizja)
    {
        Komputer komputer = kolizja.gameObject.GetComponent<Komputer>();

        if (komputer && !listaJednostekPrzeciwnika.Contains(komputer))
        {
            listaJednostekPrzeciwnika.Add(komputer);
        }
    }

    private void OnTriggerExit(Collider kolizja)
    {
        Komputer komputer = kolizja.gameObject.GetComponent<Komputer>();

        if (komputer && listaJednostekPrzeciwnika.Contains(komputer))
        {
            listaJednostekPrzeciwnika.Remove(komputer);
        }
    }

    void ZaktualizujWidok()
    {
        Komputer komputer = NajblizszaJednostka;

        if (komputer)
        {
            cel = komputer.transform;
            polecenie = Jednostka.Polecenie.gon;
        }
    }
}