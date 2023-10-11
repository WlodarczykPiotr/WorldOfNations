using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PasekZycia : MonoBehaviour
{
    Slider pasek;
    new const string tag = "pasek";

    Jednostka jednostka;

    [SerializeField]
    Vector3 bufor = Vector3.zero;

    Transform rodzic;

    void Awake()
    {
        pasek = GetComponent<Slider>();

        rodzic = transform.parent;

        jednostka = GetComponentInParent<Jednostka>();

        GameObject plotno = GameObject.FindGameObjectWithTag(tag);

        if (plotno)
        {
            transform.SetParent(plotno.transform);
        }
    }

    void Update()
    {
        if (!rodzic)
        {
            Destroy(gameObject);
            return;
        }

        if (jednostka)
        {
            pasek.value = jednostka.PasekZakres;
        }
        
        transform.position = rodzic.position + bufor;
    }
}