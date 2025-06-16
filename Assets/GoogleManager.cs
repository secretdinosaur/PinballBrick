using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Text;
using TMPro;
public class GoogleManager : MonoBehaviour
{
    public static GoogleManager instance;
    public bool isSignIn = false;
    Toggle isSignInToggle;
    TMP_Text GoogleLoginStaus;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Login();
    }

    public void Login()
    {
        isSignInToggle = GameObject.Find("Canvas").transform.Find("Scroll View/Viewport/Content/Setting/Google/Toggle").GetComponent<Toggle>();
        GoogleLoginStaus = GameObject.Find("Canvas").transform.Find("Scroll View/Viewport/Content/Setting/Google/GoogleLoginStatus").GetComponent<TMP_Text>();

        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            isSignInToggle.isOn = true;
            isSignIn = true;
            Debug.Log("SignIn Success");
            GoogleLoginStaus.text = "SignIn Success";
            // Continue with Play Games Services
        }
        else
        {
            isSignInToggle.isOn = false;
            isSignIn = false;
            Debug.Log("SignIn Fail");
            GoogleLoginStaus.text = "SignIn Fail";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
}

public class StatOnGoogle
{
    public Persistent_Stat Persistent_stat;
    private string cloudSaveFileName = "PinballBrick_persistentData.json";
    private ISavedGameClient savedGameClient;

    public void SavePersistentStats(Persistent_Stat stats)
    {
        Persistent_stat = stats;
        savedGameClient = PlayGamesPlatform.Instance.SavedGame;
    }

    public void SaveGame(string fileName, string data)
    {
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(data);

        savedGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, metadata) => OnSavedGameOpenedForSave(status, metadata, byteData));
    }

    private void OnSavedGameOpenedForSave(SavedGameRequestStatus status, ISavedGameMetadata game, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            savedGameClient.CommitUpdate(game, new SavedGameMetadataUpdate.Builder().Build(), data, OnGameSaved);
        }
        else
        {
            Debug.LogError("Failed to open saved game for saving.");
        }
    }

    private void OnGameSaved(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game saved successfully.");
        }
        else
        {
            Debug.LogError("Failed to save game.");
        }
    }

    public void LoadGame(string fileName)
    {
        savedGameClient.OpenWithAutomaticConflictResolution(fileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, metadata) => OnSavedGameOpenedForLoad(status, metadata));
    }

    private void OnSavedGameOpenedForLoad(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            savedGameClient.ReadBinaryData(game, OnGameLoaded);
        }
        else
        {
            Debug.LogError("Failed to open saved game for loading.");
        }
    }

    private void OnGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string loadedData = System.Text.Encoding.UTF8.GetString(data);
            Debug.Log("Loaded data: " + loadedData);
        }
        else
        {
            Debug.LogError("Failed to load game.");
        }
    }

    private void OnSaveGameOpenedToWrite(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string json = JsonUtility.ToJson(Persistent_stat);
            byte[] data = Encoding.UTF8.GetBytes(json);

            var update = new SavedGameMetadataUpdate.Builder().Build();
            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, OnSavedGameWritten);
        }
        else
        {
            Debug.LogWarning("클라우드 저장 열기 실패!");
        }
    }

    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("클라우드 저장 성공");
        }
        else
        {
            Debug.LogWarning("클라우드 저장 실패");
        }
    }

    public void LoadPersistentStats()
    {
        OpenSaveGameForLoad();
    }

    private void OpenSaveGameForLoad()
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;

        saveGameClient.OpenWithAutomaticConflictResolution(
            cloudSaveFileName,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            OnSaveGameOpenedToRead);
    }

    private void OnSaveGameOpenedToRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
        }
        else
        {
            Debug.LogWarning("클라우드 불러오기 실패!");
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (data == null || data.Length == 0)
            {
                Debug.Log("클라우드에 저장된 데이터가 없음. 기본값 저장 시작.");
                SaveDefaultPersistentStat();
            }
            else
            {
                string json = Encoding.UTF8.GetString(data);
                Persistent_stat = JsonUtility.FromJson<Persistent_Stat>(json);
                Debug.Log("클라우드 데이터 불러오기 성공");
            }
        }
        else
        {
            Debug.LogWarning("클라우드 데이터 읽기 실패! 기본값 저장 시도.");
            SaveDefaultPersistentStat();
        }
    }

    private void SaveDefaultPersistentStat()
    {
        Persistent_Stat initialStats = new Persistent_Stat
        {
            AttackDamage = 5f,
            EffectDamage = 5f,
            StartingBall = 5,
            BonusExp = 1f,
            CriticalChance = 0.0f,
            CriticalDamage = 1.1f,
            BonusGold = 1f,
            Gold = 0f,

            AttackDamageMultiplier = 1f,
            EffectDamageMultiplier = 1f,

            AttackDamageLevel = 1,
            EffectDamageLevel = 1,
            BonusEXPLevel = 1,
            BonusGoldLevel = 1,
            CriChanceLevel = 1,
            CriDamageLevel = 1,
            StartingBallLevel = 1
        };
        SavePersistentStats(initialStats);
    }
}
