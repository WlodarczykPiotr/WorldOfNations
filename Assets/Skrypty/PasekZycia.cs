using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasekZycia : MonoBehaviour
{
    Slider pasek;
    new const string tag = "pasek";

    Jednostka jednostka;

    [SerializeField]
    Vector3 bufor = Vector3.zero;

    Transform rodzic;

    private void Awake()
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

    private void Update()
    {
        if (!rodzic)
        {
            Destroy(gameObject);
            return;
        }

        if (jednostka)
        {
            pasek.value = jednostka.pasekZakres;
        }
        
        transform.position = rodzic.position + bufor;
    }
}