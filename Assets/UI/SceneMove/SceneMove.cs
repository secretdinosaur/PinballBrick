using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Dolsot");
    }

    public void GoToHighScore()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

}
