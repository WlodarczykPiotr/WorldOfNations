using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Robotnik : Czlowiek
{
    [Header("Robotnik")]
    [SerializeField]
    ZarzadcaZasobow.RodzajeZasobow typPrzechowywanegoZasobu;
    [SerializeField]
    ushort Kamien, Zloto, Zywnosc, Drewno;
    [SerializeField]
    ushort maksymalnyZasob = 65535;

    bool czyJestWydobywany = false;
    float randomX, randomZ;

    GameObject[] magazyn;
    GameObject[] zasob;
    GameObject punktZasobu;
    GameObject aktualnyZasob;
    GameObject punktZrzutu;
   
    Vector3 przesuniecie;
    MeshRenderer[] ekwipunek;

    protected override void Awake()
    {
        base.Awake();

        randomX = Random.Range(-1.0f, 1.0f);
        randomZ = Random.Range(-1.0f, 1.0f);

        przesuniecie = new Vector3(randomX, 0, randomZ);

        ekwipunek = new MeshRenderer[3];
        ekwipunek = gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        StartCoroutine(LicznikZasobow());
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (aktualnyZasob == null)
        {
            czyJestWydobywany = false;
            ZnajdzNajblizszyZasob();
            polecenie = Jednostka.Polecenie.idz;
        }
    
        if (Kamien >= maksymalnyZasob || Zloto >= maksymalnyZasob || Zywnosc >= maksymalnyZasob || Drewno >= maksymalnyZasob)
        {
            czyJestWydobywany = false;
            ZnajdzNajblizszyMagazyn();
            polecenie = Jednostka.Polecenie.idz;

            if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.kamien) ekwipunek[0].enabled = true;
            if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.drewno) ekwipunek[1].enabled = true;
            if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.zywnosc) ekwipunek[2].enabled = true;
            if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.zloto) ekwipunek[3].enabled = true;
        }
    }

    IEnumerator LicznikZasobow()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
    
            if (czyJestWydobywany)
            {
                if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.kamien)
                {
                    Kamien++;
                    if (Kamien < maksymalnyZasob) animator.SetTrigger("Eksploatacja");
                }
                else if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.zloto)
                {
                    Zloto++;
                    if (Zloto < maksymalnyZasob) animator.SetTrigger("Eksploatacja");
                }
                else if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.zywnosc)
                {
                    Zywnosc++;
                    if (Zywnosc < maksymalnyZasob) animator.SetTrigger("Zrywanie");
                }
                else if (typPrzechowywanegoZasobu == ZarzadcaZasobow.RodzajeZasobow.drewno)
                {
                    Drewno++;
                    if (Drewno < maksymalnyZasob) animator.SetTrigger("Eksploatacja");
                }
            }
        }
    }
    
    GameObject NajblizszyMagazynAlboZasob(GameObject[] magazyn)
    {
        GameObject najblizszyMagazynAlboZasob = null;
        float najblizszyDystans = Mathf.Infinity;
    
        Vector3 pozycjaRobotnika = transform.position;
    
        foreach (GameObject punktZrzutuAlboZasobu in magazyn)
        {
            Vector3 kierunek = punktZrzutuAlboZasobu.transform.position - pozycjaRobotnika;
            float dystans = kierunek.sqrMagnitude;
    
            if (dystans < najblizszyDystans)
            {
                najblizszyDystans = dystans;
                //Debug.Log("Najblizszy dystans: " + najblizszyDystans);
                najblizszyMagazynAlboZasob = punktZrzutuAlboZasobu;
                //Debug.Log("Najblizszy magazyn: " + najblizszyMagazynAlboZasob);
            }
        }
    
        return najblizszyMagazynAlboZasob;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        GameObject uderzonyObiekt = other.gameObject;

        transform.LookAt(uderzonyObiekt.transform);

        if (uderzonyObiekt.tag == "Zasoby")
        {
            czyJestWydobywany = true;
            uderzonyObiekt.GetComponent<ZarzadcaZasobow>().gornicy++;
            typPrzechowywanegoZasobu = uderzonyObiekt.GetComponent<ZarzadcaZasobow>().rodzajZasobu;
            aktualnyZasob = uderzonyObiekt;
        }
    
        if (uderzonyObiekt.tag == "Magazyn" && Zywnosc > 0)
        {
            Surowce.DodajZywnosc(Zywnosc);
            Zywnosc = 0;
    
            if (Kamien > 0)
            {
                Surowce.DodajKamien(Kamien);
                Kamien = 0;
            }
    
            if (Zloto > 0)
            {
                Surowce.DodajZloto(Zloto);
                Zloto = 0;
            }
    
            if (Drewno > 0)
            {
                Surowce.DodajDrewno(Drewno);
                Drewno = 0;
            }
    
            if (aktualnyZasob != null)
            {
                nawigacja.destination = aktualnyZasob.transform.position + przesuniecie;
            }
            else
            {
                ZnajdzNajblizszyZasob();
            }

            foreach (MeshRenderer zasob in ekwipunek)
            {
                zasob.enabled = false;
            }
            
        }
        else if (uderzonyObiekt.tag == "Magazyn" && Kamien > 0)
        {
            Surowce.DodajKamien(Kamien);
            Kamien = 0;
    
            if (Zywnosc > 0)
            {
                Surowce.DodajZywnosc(Zywnosc);
                Zywnosc = 0;
            }
            if (Zloto > 0)
            {
                Surowce.DodajZloto(Zloto);
                Zloto = 0;
            }
    
            if (Drewno > 0)
            {
                Surowce.DodajDrewno(Drewno);
                Drewno = 0;
            }
    
            if (aktualnyZasob != null)
            {
                nawigacja.destination = aktualnyZasob.transform.position + przesuniecie;
            }
            else
            {
                ZnajdzNajblizszyZasob();
            }

            foreach (MeshRenderer zasob in ekwipunek)
            {
                zasob.enabled = false;
            }
        }
        else if (uderzonyObiekt.tag == "Magazyn" && Zloto > 0)
        {
            Surowce.DodajZloto(Zloto);
            Zloto = 0;
    
            if (Kamien > 0)
            {
                Surowce.DodajKamien(Kamien);
                Kamien = 0;
            }
    
            if (Zywnosc > 0)
            {
                Surowce.DodajZywnosc(Zywnosc);
                Zywnosc = 0;
            }
    
            if (Drewno > 0)
            {
                Surowce.DodajDrewno(Drewno);
                Drewno = 0;
            }
    
            if (aktualnyZasob != null)
            {
                nawigacja.destination = aktualnyZasob.transform.position + przesuniecie;
            }
            else
            {
                ZnajdzNajblizszyZasob();
            }

            foreach (MeshRenderer zasob in ekwipunek)
            {
                zasob.enabled = false;
            }
        }
        else if (uderzonyObiekt.tag == "Magazyn" && Drewno > 0)
        {
            Surowce.DodajDrewno(Drewno);
            Drewno = 0;
    
            if (Kamien > 0)
            {
                Surowce.DodajKamien(Kamien);
                Kamien = 0;
            }
    
            if (Zloto > 0)
            {
                Surowce.DodajZloto(Zloto);
                Zloto = 0;
            }
    
            if (Zywnosc > 0)
            {
                Surowce.DodajZywnosc(Zywnosc);
                Zywnosc = 0;
            }
    
            if (aktualnyZasob != null)
            {
                nawigacja.destination = aktualnyZasob.transform.position + przesuniecie;
            }
            else
            {
                ZnajdzNajblizszyZasob();
            }

            foreach (MeshRenderer zasob in ekwipunek)
            {
                zasob.enabled = false;
            }
        }
    }
    
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        GameObject uderzonyObiekt = other.gameObject;
    
        if (uderzonyObiekt.tag == "Zasoby")
        {
            czyJestWydobywany = false;
            uderzonyObiekt.GetComponent<ZarzadcaZasobow>().gornicy--;
        }
    }
    
    void ZnajdzNajblizszyMagazyn()
    {
        magazyn = GameObject.FindGameObjectsWithTag("Magazyn");
        punktZrzutu = NajblizszyMagazynAlboZasob(magazyn);

        if (punktZrzutu != null) nawigacja.destination = punktZrzutu.transform.position;
        magazyn = null;
    }
    
    void ZnajdzNajblizszyZasob()
    {
        zasob = GameObject.FindGameObjectsWithTag("Zasoby");
        punktZasobu = NajblizszyMagazynAlboZasob(zasob);

        if (punktZasobu != null) nawigacja.destination = punktZasobu.transform.position + przesuniecie;
        zasob = null;
    }
}