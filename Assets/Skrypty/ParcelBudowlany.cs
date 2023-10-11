using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class ParcelBudowlany : MonoBehaviour
{
    [SerializeField]
    Color zielony = Color.green, czerwony = Color.red;

    [SerializeField]
    Vector3 wielkoscPlacera = Vector3.zero, przesunieciePola = Vector3.zero, pozycjaPrzesuniecia = Vector3.zero;

    [SerializeField]
    float odleglosc = 1;

    [SerializeField]
    LayerMask warstwaPlacera = -1;

    Vector3 pozycjaStartowa;

    Renderer render;
    NavMeshHit siatka;

    void Awake()
    {
        pozycjaStartowa = transform.position;
        render = GetComponentInChildren<Renderer>(true);
    }

    void Update()
    {
        render.sharedMaterial.color = CzyMoznaBudowac() ? zielony : czerwony; 
    }

    public bool CzyMoznaBudowac()
    {
        if(!Physics.CheckBox(transform.position + przesunieciePola, wielkoscPlacera, transform.rotation, warstwaPlacera))
        {
            if (NavMesh.SamplePosition(transform.position, out siatka, odleglosc, NavMesh.AllAreas))
            {
                return true;
            }
        }

        return false;
    }

    public void UstawPozycje(Vector3 pozycja)
    {
        transform.position = pozycja + pozycjaPrzesuniecia;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + przesunieciePola, wielkoscPlacera);
    }
}