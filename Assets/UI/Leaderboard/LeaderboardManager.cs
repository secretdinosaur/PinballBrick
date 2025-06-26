using GooglePlayGames.BasicApi;
using GooglePlayGames;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    
    public GameObject RankPanel;
    public List<RankPanel> Ranking;
    private GameObject MyRank;

    private GameObject RankPanelParent;
    private GameObject MyRankPanelParent;

    private const int PageSize = 25;
    private int _scoreOffset;
    private HashSet<string> _loadedUserIds = new HashSet<string>();

    [SerializeField]
    string userId;
    [SerializeField]
    string userName;

    private void Awake()
    {
        instance = this;
        List<RankPanel> Ranking = new List<RankPanel>();
        if (SceneManager.GetActiveScene().name == "Main")
        {
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                userId = PlayGamesPlatform.Instance.GetUserId();
                userName = PlayGamesPlatform.Instance.GetUserDisplayName();
                LoadAllTopScores();
            }
            else
            {
                userId = "local";
                userName = "localUser";
            }

            RankPanelParent = GameObject.Find("Canvas").transform.Find("Scroll View/Viewport/Content/Ranking/TotalRanking/Scroll View/Viewport/Content").gameObject;
            MyRankPanelParent = GameObject.Find("Canvas").transform.Find("Scroll View/Viewport/Content/Ranking/MyRanking").gameObject;
            LoadMyRank();
        }
    }

    public void LoadMyRank()
    {
        MyRank = Instantiate(RankPanel, MyRankPanelParent.transform);
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.LoadScores(
                GPGSIds.leaderboard_point,
                LeaderboardStart.PlayerCentered,
                1,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                (LeaderboardScoreData data) => {
                    if (data.Valid && data.Scores.Length > 0)
                    {
                        long playerScore = data.PlayerScore.value;
                        int playerRank = data.PlayerScore.rank;

                        MyRank.GetComponent<RankPanel>().ChangeInformation(playerRank, userName, playerScore);
                    }
            });
        }
        else
        {
            MyRank.GetComponent<RankPanel>().ChangeInformation(1, userName, PlayerPrefs.GetInt("HighScore"));
        }
    }

    public void LoadAllTopScores()
    {
        _scoreOffset = 0;
        foreach (var panel in Ranking)
            Destroy(panel.gameObject);
        Ranking.Clear();

        LoadScoresPage(null);
    }

    private void LoadScoresPage(ScorePageToken pageToken)
    {
        if (pageToken == null)
        {
            PlayGamesPlatform.Instance.LoadScores(
                GPGSIds.leaderboard_point,
                LeaderboardStart.TopScores,
                PageSize,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                OnScoresLoaded);
        }
        else
        {
            PlayGamesPlatform.Instance.LoadMoreScores(
                pageToken,
                PageSize,
                OnScoresLoaded);
        }
    }

    private void OnScoresLoaded(LeaderboardScoreData data)
    {
        Debug.Log($"LoadScores callback. valid={data.Valid}, returned={data.Scores.Length}");
        if (!data.Valid || data.Scores.Length == 0 || data.Scores == null)
            return;

        var newUserIds = data.Scores
            .Select(s => s.userID)
            .Where(id => !_loadedUserIds.Contains(id))
            .Distinct()
            .ToArray();

        if (newUserIds.All(item => item == null))
            return;

        int pageCount = data.Scores.Length;
        for (int i = 0; i < pageCount; i++)
        {
            GameObject Rank = Instantiate(RankPanel, RankPanelParent.transform);
            Ranking.Add(Rank.GetComponent<RankPanel>());
        }

        foreach (var id in newUserIds)
        {
            _loadedUserIds.Add(id);
        }
        PlayGamesPlatform.Instance.LoadUsers(newUserIds, users =>
        {
            for (int i = 0; i < pageCount; i++)
            {
                var scoreEntry = data.Scores[i];
                string playerId = scoreEntry.userID;
                long playerScore = scoreEntry.value;
                string playerName = users.FirstOrDefault(u => u.id == playerId)?.userName ?? "Unknown";

                int globalRankIndex = _scoreOffset + i;
                Ranking[globalRankIndex]
                    .ChangeInformation(globalRankIndex + 1, playerName, playerScore);

                Debug.Log($"{globalRankIndex + 1}. {playerName} – {playerScore}");
            }
            
            _scoreOffset += pageCount;
            if (_scoreOffset < 100 && data.NextPageToken != null)
            {
                LoadScoresPage(data.NextPageToken);
            }
            else
            {
                Debug.Log("All pages are loaded");
            }
        });
    }

    public void RecordScore(long Score)
    {
        PlayGamesPlatform.Instance.ReportScore(Score, GPGSIds.leaderboard_point, (bool success) => {

            if (success)
            {
                Debug.Log("Save score successfully");
                if (PlayerPrefs.GetInt("HighScore") < ScoreManager.Instance.TotalScore)
                {
                    PlayerPrefs.SetInt("HighScore", ScoreManager.Instance.TotalScore);
                }
            }
            else
            {
                if (!PlayerPrefs.HasKey("HighScore"))
                {
                    PlayerPrefs.SetInt("HighScore", ScoreManager.Instance.TotalScore);
                    return;
                }
                if (PlayerPrefs.GetInt("HighScore") < ScoreManager.Instance.TotalScore)
                {
                    Debug.Log("HighScore!\nSave score fail. Save score to local");
                    PlayerPrefs.SetInt("HighScore", ScoreManager.Instance.TotalScore);
                }
            }
        });
    }

    public void OpenGoogleLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_point);
    }
}
