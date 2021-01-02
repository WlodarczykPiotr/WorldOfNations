using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Informacja : MonoBehaviour
{
    Transform kameraPozycja;

    void Awake()
    {
        kameraPozycja = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(kameraPozycja);

        Vector3 rotacja = transform.localEulerAngles;

        rotacja.y = 180;
        transform.localEulerAngles = rotacja;

        if (KontrolerKamery.kontrolerKamery.obrot == 1)
        {
            rotacja.y = 0;
            transform.localEulerAngles = rotacja;
        }
    }
}
