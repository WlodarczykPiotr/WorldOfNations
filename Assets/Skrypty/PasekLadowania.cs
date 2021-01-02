using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PasekLadowania : MonoBehaviour
{
    public GameObject ekranSzkolenia;
    public Slider suwak;
    public Text postepWProcentach;

    public void SzkolJednostke(int sceneIndex)
    {
        StartCoroutine(SzkolAsynchronicznie(sceneIndex));
    }

    IEnumerator SzkolAsynchronicznie(int sceneIndex)
    {
        AsyncOperation operacja = SceneManager.LoadSceneAsync(sceneIndex);

        ekranSzkolenia.SetActive(true);

        while (!operacja.isDone)
        {
            float postep = Mathf.Clamp01(operacja.progress / 0.9f);

            suwak.value = postep;
            postepWProcentach.text = postep * 100f + "%";

            yield return null;
        }
    }
}
