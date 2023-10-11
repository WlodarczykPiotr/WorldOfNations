using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Surowce : MonoBehaviour
{
    [SerializeField]
    private ushort zywnosc, drewno, kamien, zloto, maksymalnaIlosc = 65535;

    private static Surowce surowiec;

    Text tekst;

    Texture[] ikonaSurowca = new Texture[4];
    Rect[] pozycjaIkonySurowca = new Rect[4];

    String[] zasob = new String[4] { "Zywnosc", "Drewno", "Kamien", "Zloto" };
    float[] pozycja = new float[4] { Screen.width - 540, Screen.width - 420, Screen.width - 300, Screen.width - 180 };

    void Awake()
    {
        surowiec = this;
        tekst = GetComponentInChildren<Text>(true);
    }

    void Start()
    {
        for (int i = 0; i < ikonaSurowca.Length; i++)
        {
            ikonaSurowca[i] = Resources.Load<Texture>(zasob[i]);
            pozycjaIkonySurowca[i] = new Rect(pozycja[i], 20, 120, 40);
        }
    }
    void OnGUI()
    {
        for (int i = 0; i < ikonaSurowca.Length; i++)
        {
            GUI.DrawTexture(pozycjaIkonySurowca[i], ikonaSurowca[i]);
        }
    }

    void Update()
    {
        tekst.text = "Żywność: " + zywnosc + "   Drewno: " + drewno + "   Kamień: " + kamien + "   Złoto: " + zloto;
    }

    public static bool DodajZywnosc(ushort wartosc)
    {
        surowiec.zywnosc += wartosc;

        if (surowiec.zywnosc > surowiec.maksymalnaIlosc)
        {
            surowiec.zywnosc = surowiec.maksymalnaIlosc;

            return false;
        }

        return true;
    }

    public static bool DodajDrewno(ushort wartosc)
    {
        surowiec.drewno += wartosc;

        if (surowiec.drewno > surowiec.maksymalnaIlosc)
        {
            surowiec.drewno = surowiec.maksymalnaIlosc;

            return false;
        }

        return true;
    }

    public static bool DodajKamien(ushort wartosc)
    {
        surowiec.kamien += wartosc;

        if (surowiec.kamien > surowiec.maksymalnaIlosc)
        {
            surowiec.kamien = surowiec.maksymalnaIlosc;

            return false;
        }

        return true;
    }

    public static bool DodajZloto(ushort wartosc)
    {
        surowiec.zloto += wartosc;

        if (surowiec.zloto > surowiec.maksymalnaIlosc)
        {
            surowiec.zloto = surowiec.maksymalnaIlosc;

            return false;
        }

        return true;
    }

    public static bool UjmijZywnosc(ushort wartosc)
    {
        if (surowiec.zywnosc < wartosc)
        {
            return false;
        }

        surowiec.zywnosc -= wartosc;

        return true;
    }

    public static bool UjmijDrewno(ushort wartosc)
    {
        if (surowiec.drewno < wartosc)
        {
            return false;
        }

        surowiec.drewno -= wartosc;

        return true;
    }

    public static bool UjmijKamien(ushort wartosc)
    {
        if (surowiec.kamien < wartosc)
        {
            return false;
        }

        surowiec.kamien -= wartosc;

        return true;
    }

    public static bool UjmijZloto(ushort wartosc)
    {
        if (surowiec.zloto < wartosc)
        {
            return false;
        }

        surowiec.zloto -= wartosc;

        return true;
    }

    public static bool CzyStac(ushort zywnosc, ushort drewno, ushort kamien, ushort zloto)
    {
        if ((zywnosc <= surowiec.zywnosc) && (drewno <= surowiec.drewno) && (kamien <= surowiec.kamien) && (zloto <= surowiec.zloto))
        {
            return true;
        }

        return false;
    }
}