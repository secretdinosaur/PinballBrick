using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public GameObject Loading;
    public void GoToMain()
    {
        StartCoroutine(LoadScene("Main"));
    }

    public void GoToGame()
    {
        StartCoroutine(LoadScene("Dolsot"));
    }

    public void GoToTitle()
    {
        StartCoroutine(LoadScene("Title"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);
        GameObject LoadingPanel = Instantiate(Loading, GameObject.Find("Canvas").transform);
        Slider slider = LoadingPanel.GetComponentInChildren<Slider>();
        while (!asyncOper.isDone)
        {
            slider.value = asyncOper.progress;
            yield return null;
        }
    }
}
