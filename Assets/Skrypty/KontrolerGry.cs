using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class KontrolerGry : MonoBehaviour
{
    [SerializeField]
    GameObject panelKoncowy = null, panelWaluty = null, panelJednostek = null, panelStatystyk = null;
    [SerializeField]
    KontrolerKamery kontrolerKamery = null;
    [SerializeField]
    EkranPauzy ekranPauzy = null;

    IList<Czlowiek> listaJednostekGracza = new List<Czlowiek>();
    IList<Komputer> listaJednostekPrzeciwnika = new List<Komputer>();

    Text tekst, statystyka;

    static KontrolerGry kontrolerGry;

    public static IList<Czlowiek> ListaJednostekGracza { get { return kontrolerGry.listaJednostekGracza; } }
    public static IList<Komputer> ListaJednostekPrzeciwnika { get { return kontrolerGry.listaJednostekPrzeciwnika; } }

    protected byte LicznikJednostekGracza { get; private set; } = 0;
    protected byte LicznikJednostekPrzeciwnika { get; private set; } = 0;

    void Awake()
    {
        kontrolerGry = this;
        tekst = panelKoncowy.GetComponentInChildren<Text>(true);
        statystyka = panelStatystyk.GetComponentInChildren<Text>(true);
    }

    void Update()
    {
        ListaUporzadkowana(listaJednostekGracza);
        ListaUporzadkowana(listaJednostekPrzeciwnika);

        if (ListaJednostekPrzeciwnika.Count <= 0)
        {
            Wygrana();
        }
        else if (ListaJednostekGracza.Count <= 0)
        {
            Przegrana();
        }

        if (LicznikJednostekGracza != ListaJednostekGracza.Count)
        {
            LicznikJednostekGracza = (byte)ListaJednostekGracza.Count;
            statystyka.text = "JEDNOSTKI GRACZA: " + LicznikJednostekGracza + "\n" + "JEDNOSTKI PRZECIWNIKA: " + LicznikJednostekPrzeciwnika;
        }

        if (LicznikJednostekPrzeciwnika != listaJednostekPrzeciwnika.Count)
        {
            LicznikJednostekPrzeciwnika = (byte)ListaJednostekPrzeciwnika.Count;
            statystyka.text = "JEDNOSTKI GRACZA: " + LicznikJednostekGracza + "\n" + "JEDNOSTKI PRZECIWNIKA: " + LicznikJednostekPrzeciwnika;
        }

        //Debug.Log(LicznikJednostekGracza);
        //Debug.Log(LicznikJednostekPrzeciwnika);

        //statystyka.text = "JEDNOSTKI GRACZA: " + LicznikJednostekGracza + "\n" + "JEDNOSTKI PRZECIWNIKA: " + LicznikJednostekPrzeciwnika;
    }

    void ListaUporzadkowana<T>(IList<T> lista) where T : Jednostka
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i] == null || !lista[i].CzyZyje)
            {
                lista.RemoveAt(i--);
            }
        }
    }

    void KoniecGry()
    {
        enabled = false;
        kontrolerKamery.enabled = false;
        ekranPauzy.enabled = false;
        panelWaluty.SetActive(false);
        panelJednostek.SetActive(false);
        panelKoncowy.SetActive(true);
    }

    void Wygrana()
    {
        KoniecGry();
        tekst.text = "Gratulacje, Wygrales!";
    }

    void Przegrana()
    {
        KoniecGry();
        tekst.color = Color.red;
        tekst.text = "Niestety, Przegrales...";
    }
}