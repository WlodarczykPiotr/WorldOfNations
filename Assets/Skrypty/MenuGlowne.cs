using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGlowne : MonoBehaviour
{
    public void ZacznijGrac()
    {
        SceneManager.LoadScene(1);
    }

    public void WyjdzZGry()
    {
        Application.Quit();
    }

    public void Sterowanie()
    {
        SceneManager.LoadScene(2);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}