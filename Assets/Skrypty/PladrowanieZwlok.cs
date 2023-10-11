using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PladrowanieZwlok : MonoBehaviour
{
    [SerializeField]
    GameObject prefabrykat = null;

    static PladrowanieZwlok lupy;

    void Awake()
    {
        lupy = this;
    }

    public static void PokazLupy(Vector3 pozycja, int iloscZywnosci, int iloscDrewna, int iloscKamienia, int iloscZlota)
    {
        GameObject surowce = Instantiate(lupy.prefabrykat, pozycja, lupy.transform.rotation, lupy.transform);
        surowce.GetComponent<Lupy>().Zywnosc = iloscZywnosci;
        surowce.GetComponent<Lupy>().Drewno = iloscDrewna;
        surowce.GetComponent<Lupy>().Kamien = iloscKamienia;
        surowce.GetComponent<Lupy>().Zloto = iloscZlota;
    }
}