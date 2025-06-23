using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    Toggle isGooglePlayLoginToggle;
    TMP_Text isGooglePlayLoginText;
    private void Awake()
    {
        isGooglePlayLoginToggle = GameObject.Find("Canvas").transform.Find("Google/Toggle").GetComponent<Toggle>();
        isGooglePlayLoginText = GameObject.Find("Canvas").transform.Find("Google/GoogleLoginStatus").GetComponent<TMP_Text>();
    }

    private void FixedUpdate()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            isGooglePlayLoginToggle.isOn = true;
            isGooglePlayLoginText.text = "Success to Login";
        }
        else
        {
            isGooglePlayLoginToggle.isOn = false;
            isGooglePlayLoginText.text = "Fail to Login";
        }
    }
}
