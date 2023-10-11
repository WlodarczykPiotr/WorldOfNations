using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ZarzadcaZasobow : MonoBehaviour
{
    public enum RodzajeZasobow { kamien, zloto, drewno, zywnosc }
    public RodzajeZasobow rodzajZasobu = RodzajeZasobow.zywnosc;

    [SerializeField]
    private short iloscZasobu;

    public short gornicy;

    IEnumerator LicznikZasobu()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Wydobywanie();
        }
    }

    void Wydobywanie()
    {
        if (gornicy > 0)
        {
            iloscZasobu -= gornicy;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LicznikZasobu());
    }

    // Update is called once per frame
    void Update()
    {
        if (iloscZasobu <= 0)
        {
            Destroy(gameObject);
        }
    }
}