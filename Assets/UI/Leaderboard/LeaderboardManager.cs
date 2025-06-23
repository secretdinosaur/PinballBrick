using GooglePlayGames.BasicApi;
using GooglePlayGames;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;
using UnityEngine.SocialPlatforms;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    
    public GameObject RankPanel;
    private RankPanel[] Ranking;
    private GameObject MyRank;

    string userId;
    string userName;

    private void Awake()
    {
        instance = this;

        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            string userId = PlayGamesPlatform.Instance.GetUserId();
            string userName = PlayGamesPlatform.Instance.GetUserDisplayName();
        }
        else
        {
            userId = "local";
            userName = "local user";
        }

        RankingSetting();
    }

    private void RankingSetting()
    {
        GameObject RankPanelParent = GameObject.Find("Canvas").transform.Find("Scroll View/Viewport/Content/Ranking/TotalRanking/Scroll View/Viewport/Content").gameObject;
        GameObject MyRankPanelParent = GameObject.Find("Canvas").transform.Find("Scroll View/Viewport/Content/Ranking/MyRanking").gameObject;
        Ranking = new RankPanel[PlayGamesPlatform.Instance.IsAuthenticated() ? 100 : 1];

        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.LoadScores(
            GPGSIds.leaderboard_point,
            LeaderboardStart.TopScores,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            data => {
                if (data.Valid)
                {
                    targetCount = (int)data.ApproximateCount;
                }
                else
                {
                    Debug.LogWarning("LoadScores fail");
                }
            });
        }
        else
        {
            targetCount = 1;
        }
        
        for (int i = 0; i < Ranking.Length; i++)
        {
            GameObject Rank = Instantiate(RankPanel, RankPanelParent.transform);
        }
        MyRank = Instantiate(RankPanel, MyRankPanelParent.transform);

        LoadTopScores();
        LoadMyRank();
    }

    public void LoadTopScores()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            LoadTop100Scores();
        }
        else
        {
            Ranking[0].ChangeInformation(1, userName, PlayerPrefs.GetInt("HighScore"));
        }
    }

    public void LoadMyRank()
    {
        if(PlayGamesPlatform.Instance.IsAuthenticated())
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
                        string playerId = userName;
                        long playerScore = data.Scores[0].value;
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

    List<IScore> allScores;
    int targetCount = 100;

    public void LoadTop100Scores()
    {
        allScores = new List<IScore>();

        PlayGamesPlatform.Instance.LoadScores(
            GPGSIds.leaderboard_point,
            LeaderboardStart.TopScores,
            25,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (LeaderboardScoreData data) => {
                if (data.Valid)
                {
                    allScores.AddRange(data.Scores);
                    if (data.NextPageToken != null && allScores.Count < targetCount)
                    {
                        LoadMoreScores(data.NextPageToken);
                    }
                    else
                    {
                        LoadUserProfilesAndUpdate();
                    }
                }
            });
    }

    void LoadMoreScores(ScorePageToken token)
    {
        PlayGamesPlatform.Instance.LoadMoreScores(token, 25, (LeaderboardScoreData data) =>
        {
            if (data.Valid)
            {
                allScores.AddRange(data.Scores);

                if (data.NextPageToken != null && allScores.Count < targetCount)
                {
                    LoadMoreScores(data.NextPageToken);
                }
                else
                {
                    LoadUserProfilesAndUpdate();
                }
            }
        });
    }

    void LoadUserProfilesAndUpdate()
    {
        int count = Mathf.Min(allScores.Count, targetCount);
        string[] userIds = new string[count];

        for (int i = 0; i < count; i++)
        {
            userIds[i] = allScores[i].userID;
        }

        PlayGamesPlatform.Instance.LoadUsers(userIds, (users) =>
        {
            for (int i = 0; i < count; i++)
            {
                string playerId = allScores[i].userID;
                long playerScore = allScores[i].value;
                string playerName = "";

                foreach (var user in users)
                {
                    if (user.id == playerId)
                    {
                        playerName = user.userName;
                        break;
                    }
                }


                Ranking[i].ChangeInformation(i + 1, playerName, playerScore);
            }
        });
    }


    public void RecordScore(long Score)
    {
        PlayGamesPlatform.Instance.ReportScore(Score, GPGSIds.leaderboard_point, (bool success) => {

            if (success)
            {
                Debug.Log("Save score successfully");
            }
            else
            {
                Debug.Log("Save score fail. Save score to local");
                if (PlayerPrefs.GetInt("HighScore") > ScoreManager.Instance.TotalScore)
                {
                    PlayerPrefs.SetInt("HighScore", ScoreManager.Instance.TotalScore);
                }
            }
        });
    }

}
