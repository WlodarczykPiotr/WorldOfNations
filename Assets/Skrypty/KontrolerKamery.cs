using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KontrolerKamery : MonoBehaviour
{
    public static KontrolerKamery kontrolerKamery;

    Camera kamera;
    RectTransform poleWyboru;
    Rect prostokat, wartoscPolaWyboru;

    public float wysokoscTerenu;
    public float predkoscZoomowania;
    public float predkoscKamery;

    bool czyKursorJestNaEkranie;

    public Vector2 rotacjaKamery;
    public Vector2 zoomKamery;

    [Range(0, 0.2f)]
    public float progKursora = 0;

    [Range(0, 1)]
    public float stopienZoomu = 0;

    [SerializeField]
    LayerMask warstwaPolecenia = -1, warstwaBudynku = 0;

    Vector2 pozycjaMyszy;
    Vector2 pozycjaMyszyNaEkranie;
    Vector2 stopienScrolla;
    Vector2 wejscieKlawiatury;

    List<InterfejsWyboru> zaznaczoneJednostki = new List<InterfejsWyboru>();

    ParcelBudowlany obszar;
    GameObject budynek;
    RaycastHit punkt;
    Ray promien;
    public byte obrot = 0;
    float x, y, z;

    private void Awake()
    {
        kontrolerKamery = this;
        poleWyboru = GetComponentInChildren<Image>(true).transform as RectTransform;
        kamera = GetComponent<Camera>();
        poleWyboru.gameObject.SetActive(false);
    }

    private void Start()
    {
        poleWyboru.gameObject.SetActive(false);
        obszar = GameObject.FindObjectOfType<ParcelBudowlany>();
        obszar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (obrot == 0)
        {
            RuchKamery(1);
        }
        else
        {
            RuchKamery(-1);
        }
        
        Zoom();
        Klikniecie();
        Obszar();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (obrot == 0)
            {
                x = transform.position.x;
                y = transform.position.y;
                z = transform.position.z;

                transform.position = new Vector3(x, y, z + 30);
                transform.rotation = new Quaternion(0, 1, 0, 0);

                obrot = 1;
            }
            else if (obrot == 1)
            {
                x = transform.position.x;
                y = transform.position.y;
                z = transform.position.z;

                transform.position = new Vector3(x, y, z - 30);
                transform.rotation = new Quaternion(0, 0, 0, 0);

                obrot = 0;
            }
        }
    }

    void RuchKamery(sbyte obrot)
    {
        wejscieKlawiatury = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        pozycjaMyszy = Input.mousePosition;
        pozycjaMyszyNaEkranie = kamera.ScreenToViewportPoint(pozycjaMyszy);
        czyKursorJestNaEkranie = pozycjaMyszyNaEkranie.x >= 0 && pozycjaMyszyNaEkranie.x <= 1 &&
                                 pozycjaMyszyNaEkranie.y >= 0 && pozycjaMyszyNaEkranie.y <= 1;

        Vector2 kierunekRuchu = wejscieKlawiatury;

        if (czyKursorJestNaEkranie)
        {
            if (pozycjaMyszyNaEkranie.x < progKursora) kierunekRuchu.x -= 1 - pozycjaMyszyNaEkranie.x / progKursora;
            if (pozycjaMyszyNaEkranie.x > 1 - progKursora) kierunekRuchu.x += 1 - (1 - pozycjaMyszyNaEkranie.x) / progKursora;
            if (pozycjaMyszyNaEkranie.y < progKursora) kierunekRuchu.y -= 1 - pozycjaMyszyNaEkranie.y / progKursora;
            if (pozycjaMyszyNaEkranie.y > 1 - progKursora) kierunekRuchu.y += 1 - (1 - pozycjaMyszyNaEkranie.y) / progKursora;
        }

        Vector3 przyrostPozycji = new Vector3(kierunekRuchu.x * obrot, 0, kierunekRuchu.y * obrot);
        przyrostPozycji *= predkoscKamery * Time.deltaTime;
        transform.localPosition += przyrostPozycji;
    }

    void Zoom()
    {
        stopienScrolla = Input.mouseScrollDelta;
        float przyrostZoomu = stopienScrolla.y * predkoscZoomowania * Time.deltaTime;
        stopienZoomu = Mathf.Clamp(stopienZoomu + przyrostZoomu, 0, 1);

        Vector3 pozycja = transform.localPosition;
        pozycja.y = Mathf.Lerp(zoomKamery.y, zoomKamery.x, stopienZoomu) + wysokoscTerenu;
        transform.localPosition = pozycja;

        Vector3 rotacja = transform.localEulerAngles;
        rotacja.x = Mathf.Lerp(rotacjaKamery.y, rotacjaKamery.x, stopienZoomu);
        transform.localEulerAngles = rotacja;

        // Lerp jest funkcją matematyczną, która interpoluje (oblicza) wynik ze skrajnych 2 wartości
    }

    void Klikniecie()
    {
        if (Input.GetMouseButtonDown(0))
        {
            poleWyboru.gameObject.SetActive(true);
            prostokat.position = pozycjaMyszy;

            Buduj();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            poleWyboru.gameObject.SetActive(false);

        }

        if (Input.GetMouseButton(0))
        {
            prostokat.size = pozycjaMyszy - prostokat.position;
            wartoscPolaWyboru = wartoscBezwzgledna(prostokat);
            poleWyboru.anchoredPosition = wartoscPolaWyboru.position;
            poleWyboru.sizeDelta = wartoscPolaWyboru.size;

            if (wartoscPolaWyboru.size.x != 0 || wartoscPolaWyboru.size.y != 0)
            {
                Zaznaczenie();
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            WydajPolecenie();

            budynek = null;
        }

        if (Input.GetMouseButtonDown(2))
        {
            stopienZoomu = 0.5f;
        }
    }

    Rect wartoscBezwzgledna(Rect prostokat)
    {
        if (prostokat.width < 0)
        {
            prostokat.x += prostokat.width;
            prostokat.width *= -1;
        }

        if (prostokat.height < 0)
        {
            prostokat.y += prostokat.height;
            prostokat.height *= -1;
        }

        return prostokat;
    }

    void Zaznaczenie()
    {
        zaznaczoneJednostki.Clear();

        foreach (InterfejsWyboru zaznaczenie in Jednostka.ZaznaczoneJednostki)
        {
            if (zaznaczenie == null) continue;

            MonoBehaviour klasaPodstawowa = zaznaczenie as MonoBehaviour;
            Vector3 pozycja = klasaPodstawowa.transform.position;
            Vector3 pozycjaNaEkranie = kamera.WorldToScreenPoint(pozycja);

            bool wProstokacie = CzyPunktJestWProstokacie(wartoscPolaWyboru, pozycjaNaEkranie);

            (zaznaczenie as InterfejsWyboru).UstawZaznaczenie(wProstokacie);

            if (wProstokacie)
            {
                zaznaczoneJednostki.Add(zaznaczenie);
            }
        }
    }

    bool CzyPunktJestWProstokacie(Rect prostokat, Vector2 punkt)
    {
        return punkt.x >= prostokat.position.x && punkt.x <= (prostokat.position.x + prostokat.size.x) &&
               punkt.y >= prostokat.position.y && punkt.y <= (prostokat.position.y + prostokat.size.y);
    }

    void WydajPolecenie()
    {
        promien = kamera.ViewportPointToRay(pozycjaMyszyNaEkranie);

        if (Physics.Raycast(promien, out punkt, 1000, warstwaPolecenia))
        {
            object polecenie = null;

            if (punkt.collider is TerrainCollider)
            {
                polecenie = punkt.point;
            }
            else
            {
                polecenie = punkt.collider.gameObject.GetComponent<Jednostka>();
            }

            WydajPolecenie(polecenie, "Polecenie");
        }
    }

    void WydajPolecenie(object polecenie, string nazwaPolecenia)
    {
        foreach (InterfejsWyboru zaznaczenie in zaznaczoneJednostki)
            (zaznaczenie as MonoBehaviour).SendMessage(nazwaPolecenia, polecenie, SendMessageOptions.DontRequireReceiver);
    }

    public static void UtworzJednostki(GameObject rycerz)
    {
        kontrolerKamery.WydajPolecenie(rycerz, "UtworzJednostke");
    }

    public static void UtworzBudynek(GameObject koszary)
    {
        kontrolerKamery.budynek = koszary;
    }

    void Obszar()
    {
        obszar.gameObject.SetActive(budynek);

        if (obszar.gameObject.activeInHierarchy)
        {
            promien = kamera.ViewportPointToRay(pozycjaMyszyNaEkranie);

            if (Physics.Raycast(promien, out punkt, 1000, warstwaPolecenia))
            {
                obszar.UstawPozycje(punkt.point);
            }
        }
    }

    void Buduj()
    {
        if (budynek && obszar && obszar.isActiveAndEnabled && obszar.CzyMoznaBudowac())
        {
            Transakcja transakcja = budynek.GetComponent<Transakcja>();

            if (!transakcja || !Surowce.UjmijZywnosc(transakcja.zywnosc))
            {
                return;
            }
            else if (!transakcja || !Surowce.UjmijDrewno(transakcja.drewno))
            {
                return;
            }
            else if (!transakcja || !Surowce.UjmijKamien(transakcja.kamien))
            {
                return;
            }
            else if (!transakcja || !Surowce.UjmijZloto(transakcja.zloto))
            {
                return;
            }

            GameObject budowla = Instantiate(budynek, obszar.transform.position, obszar.transform.rotation);

            PladrowanieZwlok.PokazLupy(budowla.transform.position, -transakcja.zywnosc, -transakcja.drewno, -transakcja.kamien, -transakcja.zloto);
        }
    }
}