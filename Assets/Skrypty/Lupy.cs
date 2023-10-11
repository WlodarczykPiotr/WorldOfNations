using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Lupy : MonoBehaviour
{
    int zywnosc, drewno, kamien, zloto;

    public int Zywnosc { set { zywnosc = value; } }
    public int Drewno { set { drewno = value; } }
    public int Kamien { set { kamien = value; } }
    public int Zloto { set { zloto = value; } }

    [SerializeField]
    Vector3 buforPoczatek = Vector3.zero, buforKoniec = Vector3.zero;

    [SerializeField]
    float czasTrwania = 0;

    [SerializeField]
    Color kolorPoczatkowy = Color.green, kolorKoncowy = Color.red;

    Vector3 pozycjaPoczatkowa;
    Text tekst;
    float czas;

    void Awake()
    {
        tekst = GetComponentInChildren<Text>(true);
        pozycjaPoczatkowa = transform.position;
        czas = czasTrwania;
    }

    void Update()
    {
        czas = czas - Time.deltaTime;
        float postep = 1 - czas / czasTrwania;
        transform.position = Vector3.Lerp(pozycjaPoczatkowa + buforPoczatek, pozycjaPoczatkowa + buforKoniec, postep);

        tekst.color = Color.Lerp(kolorPoczatkowy, kolorKoncowy, postep);
        tekst.text = zywnosc + " Ż  " + drewno + " D  " + kamien + " K  " + zloto + " Z";

        if (czas <= 0)
        {
            Destroy(gameObject);
        }
    }
}