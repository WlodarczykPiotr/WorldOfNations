using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyklDniaINocy : MonoBehaviour
{
    public float predkoscSlonca = 1;
    private float mnozenie;

    void Update()
    {
        mnozenie = predkoscSlonca * Time.deltaTime;
        transform.Rotate(Vector3.up * mnozenie);
    }
}