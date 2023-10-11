using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CyklDniaINocy : MonoBehaviour
{
    [SerializeField]
    private float predkoscSlonca = 1;
    float mnozenie;

    void Update()
    {
        mnozenie = predkoscSlonca * Time.deltaTime;
        transform.Rotate(Vector3.up * mnozenie);
    }
}