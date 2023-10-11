using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MenuGlowne : MonoBehaviour
{
    void ZacznijGrac()
    {
        SceneManager.LoadScene(1);
    }

    void WyjdzZGry()
    {
        Application.Quit();
    }

    void Sterowanie()
    {
        SceneManager.LoadScene(2);
    }

    void Menu()
    {
        SceneManager.LoadScene(0);
    }
}