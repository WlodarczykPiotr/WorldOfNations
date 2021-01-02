using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //biblioteka nawigacji

// Funkcje napisane na niebiesko są funkcjami systemowymi Unity, wywołują się automatycznie w konkretnych momentach przez silnik Unity

public class Jednostka : MonoBehaviour
{
    //Serializacja to automatyczny proces przekształcania struktur danych lub stanów obiektów do formatu, który Unity może przechowywać i rekonstruować później.

    //Przeładowywanie na goraco to proces tworzenia lub edycji skryptów, gdy Edytor jest otwarty i natychmiast stosuje zachowania skryptu.Nie musisz ponownie uruchamiać aplikacji ani Edytora, aby zmiany odniosły skutek.
    //Gdy zmienisz i zapiszesz skrypt, Unity hot przeładowuje wszystkie aktualnie załadowane dane skryptu. Najpierw przechowuje wszystkie możliwe do serializacji zmienne we wszystkich załadowanych skryptach, a po załadowaniu skryptów przywraca je. Wszystkie dane, których nie można serializować, są tracone po ponownym załadowaniu.
    //Unity używa serializacji do ładowania i zapisywania scen
    //Wiele funkcji edytora Unity jest tworzonych na podstawie podstawowego systemu serializacji. Dwie rzeczy, na które należy zwrócić szczególną uwagę w przypadku serializacji, to okno Inspector i ponowne ładowanie na goraco.
    //Okno inspektora. Podczas przeglądania lub zmiany wartości GameObject Pola komponentu w oknie Inspector, Unity serializuje te dane, a następnie wyświetla je w oknie Inspector.
    //Jeśli używasz właściwości w skrypcie, żadne z metod pobierających i ustawiających właściwości nigdy nie są wywoływane podczas przeglądania lub zmiany wartości w oknach inspektora, ponieważ Unity bezpośrednio serializuje pola okna inspektora. Oznacza to, że: Podczas gdy wartości pola w oknie Inspector reprezentują właściwości skryptu, zmiany wartości w oknie Inspector nie wywołują żadnych metod pobierających i ustawiających właściwości w skrypcie

    [Header("Jednostka")]
    [SerializeField]
    GameObject pasekZycia = null;
    [SerializeField]
    float pasek, maks = 100;
    [SerializeField]
    protected float obrazeniaAtaku = 10;
    [SerializeField]
    protected float szybkoscAtaku = 1;
    [SerializeField]
    protected float odlegloscDoAtaku = 1;
    [SerializeField]
    protected float stop = 1;
    
    protected PasekZycia pasek_zycia;
    protected NavMeshAgent nawigacja;
    protected Animator animator;
    protected Transform cel; // Komponent Transform przechowuje współrzędne pozycji, rotacji i skali danego obiektu
    protected Polecenie polecenie = Polecenie.spocznij;

    float czasAtaku;

    const string SZYBKOSC = "Szybkosc";
    const string CZY_ZYJE = "Czy_zyje";
    const string WALKA = "Walka";
    const string CIOS = "Cios";
    const string KONIEC = "Koniec";

    public bool CzyZyje { get { return pasek > 0; } }
    public float pasekZakres { get { return pasek / maks; } }

    public static List<InterfejsWyboru> ZaznaczoneJednostki { get { return zaznaczoneJednostki; } }
    static List<InterfejsWyboru> zaznaczoneJednostki = new List<InterfejsWyboru>();

    private int generatorBrzdekuMiecza;
    private int generatorOdglosuSmierci;

    public enum Polecenie
    {
        spocznij, idz, sledz, atakuj, gon
    }

    protected virtual void Awake() // Funkcja ta jest wywoływana w momencie utworzenia obiektu, nawet gdy będzie on wyłączony na start
    {
        // virtual - ponieważ istnieje możliwość nadpisania tej funkcji i rozszerzenia jej o nowe możliwości

        nawigacja = GetComponent<NavMeshAgent>(); // Wczytanie komponentu nawigacji do obiektu ze skryptem
        animator = GetComponent<Animator>();

        pasek_zycia = Instantiate(pasekZycia, transform).GetComponent<PasekZycia>();
        pasek = maks;

        generatorBrzdekuMiecza = Random.Range(0,8);
        generatorOdglosuSmierci = Random.Range(0,12);
    }

    protected virtual void Start() // Funkcja ta jest wywoływana kiedy pierwszy raz jest aktualizowany dany obiekt, tzn. że jest załadowany i włączony
    {
        //Start jest wywoływany przed aktualizacją pierwszej klatki

        if (this is InterfejsWyboru)
        {
            zaznaczoneJednostki.Add(this as InterfejsWyboru);
            (this as InterfejsWyboru).UstawZaznaczenie(false);
        }
    }

    void Update() // Funkcja ta jest wywoływana z każdą klatką gry
    {
        //Aktualizacja jest wywoływana raz na klatkę

        if (cel != null)
        nawigacja.SetDestination(cel.position); // Podążanie obiektu ze skryptem nawigacji za obiektem umieszczonym w komponecie Transform

        // ustawia lub aktualizuje miejsce docelowe, uruchamiając w ten sposób obliczenia dla nowej ścieżki

        if (CzyZyje)
        {
            switch (polecenie)
            {
                case Polecenie.spocznij:
                    Spocznij();
                    break;
                case Polecenie.idz:
                    Idz();
                    break;
                case Polecenie.sledz:
                    Sledz();
                    break;
                case Polecenie.atakuj:
                    Atakuj();
                    break;
                case Polecenie.gon:
                    Gon();
                    break;
            }
        }

        Animacja();
    }

    protected virtual void Animacja() // Nadawanie wartości dla animatora
    {
        float predkosc = nawigacja.velocity.magnitude; // velocity - obecna prędkość używana przez system nawigacji, magnitude - długość wektora

        // dostosowuje bieżącą prędkość do komponentu agenta siatki nawigacji lub ustawia prędkość, aby ręcznie sterować agentem

        animator.SetFloat(SZYBKOSC, predkosc); // wyślij wartości zmiennoprzecinkowe do animatora, aby wpłynąć na przejścia
        animator.SetBool(CZY_ZYJE, CzyZyje);
    }

    protected virtual void Spocznij()
    {
        nawigacja.velocity = Vector3.zero;
    }

    protected virtual void Idz()
    {
        float dystans = Vector3.Magnitude(nawigacja.destination - transform.position);

        if (dystans <= stop)
        {
            polecenie = Polecenie.spocznij;
        }
    }

    protected virtual void Sledz()
    {
        if (cel != null)
        {
            nawigacja.SetDestination(cel.position);
        }
        else
        {
            polecenie = Polecenie.spocznij;
        }
    }

    protected virtual void Gon()
    {
        if (cel != null)
        {
            nawigacja.SetDestination(cel.position);

            float dystans = Vector3.Magnitude(nawigacja.destination - transform.position);

            if (dystans <= odlegloscDoAtaku)
            {
                polecenie = Polecenie.atakuj;
            }
        }
        else
        {
            polecenie = Polecenie.spocznij;
        }
    }

    protected virtual void Atakuj()
    {
        if (cel != null)
        {
            nawigacja.velocity = Vector3.zero;
            transform.LookAt(cel);

            float dystans = Vector3.Magnitude(nawigacja.destination - transform.position);

            if (dystans <= odlegloscDoAtaku)
            {
                if ((czasAtaku -= Time.deltaTime) <= 0)
                {
                    Atak();
                }
            }
            else
            {
                polecenie = Polecenie.gon;
            }
        }
        else
        {
            polecenie = Polecenie.spocznij;
        }
    }

    public virtual void Atak()
    {
        Jednostka jednostka = cel.GetComponent<Jednostka>();

        if (jednostka && jednostka.CzyZyje)
        {
            OdtwarzaczOdglosow.PlaySound("brzdek"+ generatorBrzdekuMiecza);
            animator.SetTrigger(WALKA);
            czasAtaku = szybkoscAtaku;
        }
        else
        {
            cel = null;
            polecenie = Polecenie.spocznij;
        }

    }

    public virtual void ZadajObrazenia()
    {
        if (cel != null)
        {
            Jednostka jednostka = cel.GetComponent<Jednostka>();

            if (jednostka && jednostka.CzyZyje)
            {
                jednostka.PrzyjmijObrazenia(obrazeniaAtaku, transform.position);
            }
        }
    }

    public virtual void PrzyjmijObrazenia(float obrazenia, Vector3 pozycjaZadawaniaObrazen)
    {
        if (CzyZyje)
        {
            pasek -= obrazenia;
        }

        if (!CzyZyje)
        {
            //Debug.Log("poszlo");

            OdtwarzaczOdglosow.PlaySound("smierc"+ generatorOdglosuSmierci);
            animator.SetTrigger(KONIEC);
            pasek_zycia.gameObject.SetActive(false);
            enabled = false;
            nawigacja.enabled = false;

            foreach (var zderzak in GetComponents<Collider>())
            {
                zderzak.enabled = false;
            }
            
            if (this is InterfejsWyboru)
            {
                zaznaczoneJednostki.Remove(this as InterfejsWyboru);
            }
        }
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, odlegloscDoAtaku);
    }

    private void OnDestroy()
    {
        if (this is InterfejsWyboru) zaznaczoneJednostki.Remove(this as InterfejsWyboru);
    }
}