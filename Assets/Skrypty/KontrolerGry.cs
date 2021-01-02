using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KontrolerGry : MonoBehaviour
{
    [SerializeField]
    GameObject panelKoncowy = null, panelWaluty = null, panelJednostek = null;

    [SerializeField]
    KontrolerKamery kontrolerKamery = null;

    [SerializeField]
    EkranPauzy ekranPauzy = null;

    List<Czlowiek> listaJednostekGracza = new List<Czlowiek>();
    List<Komputer> listaJednostekPrzeciwnika = new List<Komputer>();

    Text tekst;

    static KontrolerGry kontrolerGry;

    public static List<Czlowiek> ListaJednostekGracza { get { return kontrolerGry.listaJednostekGracza; } }
    public static List<Komputer> ListaJednostekPrzeciwnika { get { return kontrolerGry.listaJednostekPrzeciwnika; } }

    private void Awake()
    {
        kontrolerGry = this;
        tekst = panelKoncowy.GetComponentInChildren<Text>(true);
    }

    void Update()
    {
        ListaUporzadkowana(listaJednostekGracza);
        ListaUporzadkowana(listaJednostekPrzeciwnika);

        if (listaJednostekPrzeciwnika.Count <= 0)
        {
            Wygrana();
        }
        else if (listaJednostekGracza.Count <= 0)
        {
            Przegrana();
        }
    }

    void ListaUporzadkowana<T>(List<T> lista) where T : Jednostka
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