using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PladrowanieZwlok : MonoBehaviour
{
    [SerializeField]
    GameObject prefabrykat = null;

    static PladrowanieZwlok lupy;

    private void Awake()
    {
        lupy = this;
    }

    public static void PokazLupy(Vector3 pozycja, int iloscZywnosci, int iloscDrewna, int iloscKamienia, int iloscZlota)
    {
        GameObject surowce = Instantiate(lupy.prefabrykat, pozycja, lupy.transform.rotation, lupy.transform);
        surowce.GetComponent<Lupy>().zywnosc = iloscZywnosci;
        surowce.GetComponent<Lupy>().drewno = iloscDrewna;
        surowce.GetComponent<Lupy>().kamien = iloscKamienia;
        surowce.GetComponent<Lupy>().zloto = iloscZlota;
    }
}