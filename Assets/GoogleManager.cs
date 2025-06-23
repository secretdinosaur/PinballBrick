using UnityEngine;
using System.IO;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;
using System.Text;
using TMPro;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
public class GoogleManager : MonoBehaviour
{
    public static GoogleManager instance;

    void Awake()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Login();
        }
    }

    public void Login()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("SignIn Success");

            // Continue with Play Games Services
        }
        else
        {
            Debug.Log("SignIn Fail");

            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
}