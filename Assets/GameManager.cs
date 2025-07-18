using DG.Tweening;
using GoogleMobileAds.Api;
using GooglePlayGames;
using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameEnded = false;
    public bool OnChaos = false;

    public GameObject PausePanel;
    public GameObject GameEndPanel;

    TMP_Text AttackDamageStatValueText;
    TMP_Text CriticalDamageStatValueText;
    TMP_Text CriticalChanceStatValueText;
    TMP_Text EffectDamageStatValueText;
    TMP_Text EffectDurationStatValueText;
    TMP_Text GoldGainStatValueText;
    TMP_Text ExpGainStatValueText;

    TMP_Text NoticeLevelText_Pause;
    TMP_Text NoticePlaytimeText_Pause;
    TMP_Text NoticePointText_Pause;
    TMP_Text NoticeWaveText_Pause;

    TMP_Text NoticeLevelText_End;
    TMP_Text NoticePlaytimeText_End;
    TMP_Text NoticePointText_End;
    TMP_Text NoticeWaveText_End;
    TMP_Text NoticeDestroyedBricksText_End;
    TMP_Text NoticeNewRecordText;

    Sequence GameEndSequence;

    public TMP_Text WaveNoticeText;
    public TMP_Text WaveExtraNoticeText;

    private string adUnitId;
    private InterstitialAd _interstitialAd;

    private int wave;
    public int Wave
    {
        get {  return wave; }
        set 
        { 
            wave = value;
            Mathf.Clamp(wave, 1, 9999999999);
            ReachedWave = value;
        }
    }

    private int reachedWave;
    public int ReachedWave
    {
        get { return reachedWave; }
        set
        {
            if (value > reachedWave)
            {
                reachedWave = value;
                if (value > StatManager.instance.Statistics.ReachedWave)
                {
                    RecordStatistics.ReachedWave = value;
                }
            }
        }
    }
    public StatisticsSet RecordStatistics;

    void Awake()
    {
        instance = this;

        PausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        GameEndPanel = GameObject.Find("Canvas").transform.Find("GameEndPanel").gameObject;

        AttackDamageStatValueText = PausePanel.transform.Find("Stats/AttackDamageIcon/AttackDamageStatValueText").GetComponent<TMP_Text>();
        CriticalDamageStatValueText = PausePanel.transform.Find("Stats/CriticalDamageIcon/CriticalDamageStatValueText").GetComponent<TMP_Text>();
        CriticalChanceStatValueText = PausePanel.transform.Find("Stats/CriticalChanceIcon/CriticalChanceStatValueText").GetComponent<TMP_Text>();
        EffectDamageStatValueText = PausePanel.transform.Find("Stats/EffectDamageIcon/EffectDamageStatValueText").GetComponent<TMP_Text>();
        EffectDurationStatValueText = PausePanel.transform.Find("Stats/EffectDurationIcon/EffectDurationStatValueText").GetComponent<TMP_Text>();
        GoldGainStatValueText = PausePanel.transform.Find("Stats/GoldGainIcon/GoldGainStatValueText").GetComponent<TMP_Text>();
        ExpGainStatValueText = PausePanel.transform.Find("Stats/ExpGainIcon/ExpGainStatValueText").GetComponent<TMP_Text>();

        NoticeLevelText_Pause = PausePanel.transform.Find("Info/Level").GetComponent<TMP_Text>();
        NoticePlaytimeText_Pause = PausePanel.transform.Find("Info/Playtime").GetComponent<TMP_Text>();
        NoticePointText_Pause = PausePanel.transform.Find("Info/Point").GetComponent<TMP_Text>();
        NoticeWaveText_Pause = PausePanel.transform.Find("Info/Wave").GetComponent<TMP_Text>();

        NoticeLevelText_End = GameEndPanel.transform.Find("Level").GetComponent<TMP_Text>();
        NoticePlaytimeText_End = GameEndPanel.transform.Find("Playtime").GetComponent<TMP_Text>();
        NoticePointText_End = GameEndPanel.transform.Find("Point").GetComponent<TMP_Text>();
        NoticeWaveText_End = GameEndPanel.transform.Find("Wave").GetComponent<TMP_Text>();
        NoticeDestroyedBricksText_End = GameEndPanel.transform.Find("DestroyedBricks").GetComponent<TMP_Text>();
        NoticeNewRecordText = NoticePointText_End.gameObject.transform.Find("NewRecord").GetComponent<TMP_Text>();
        NoticeNewRecordText.gameObject.SetActive(false);

        WaveNoticeText = GameObject.Find("Canvas").transform.Find("WaveNoticeText").GetComponent<TMP_Text>();
        WaveExtraNoticeText = GameObject.Find("Canvas").transform.Find("WaveExtraNoticeText").GetComponent<TMP_Text>();
        
        Wave = 0;

        RecordStatistics = new StatisticsSet();

        adUnitId = "ca-app-pub-3940256099942544/1033173712";
        _interstitialAd = null;
        LoadAds();

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        WaveNoticeText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            WaveNoticeText.text = "Start In " + i;
            yield return new WaitForSeconds(1f);
        }

        SoundManager.instance.Bgm.mute = false;
        StartCoroutine(BallManager.Instance.CreateStartingBall());
        BrickManager.Instance.SpawnBrickRoutine = StartCoroutine(BrickManager.Instance.SpawnBricks());
    }

    public IEnumerator WaveNotice()
    {
        if (!BrickManager.Instance.BossSpawnCondition(Wave))
        {
            WaveNoticeText.text = "Wave " + Wave;
        }
        else
        {
            WaveNoticeText.text = "Boss Wave";
        }
        if(BrickManager.Instance.isRapid)
        {
            WaveExtraNoticeText.text = "Rapid";
            WaveExtraNoticeText.gameObject.SetActive(true);
        }
        WaveNoticeText.gameObject.SetActive(true);
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, time / 0.5f);
            Color color = WaveNoticeText.color;
            color.a = alpha;
            WaveNoticeText.color = color;
            if (WaveExtraNoticeText.gameObject.activeSelf)
            {
                Color color1 = WaveExtraNoticeText.color;
                color1.a = alpha;
                WaveExtraNoticeText.color= color1;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / 0.5f);
            Color color = WaveNoticeText.color;
            color.a = alpha;
            WaveNoticeText.color = color;
            if (WaveExtraNoticeText.gameObject.activeSelf)
            {
                Color color1 = WaveExtraNoticeText.color;
                color1.a = alpha;
                WaveExtraNoticeText.color = color1;
            }
            yield return null;
        }
        if (WaveExtraNoticeText.gameObject.activeSelf)
        {
            WaveExtraNoticeText.gameObject.SetActive(false);
        }
        BrickManager.Instance.WaveRoutine = null;
        WaveNoticeText.gameObject.SetActive(false);
    }

    public void GameExit()
    {
        SoundManager.instance.SaveVolume();
        Fever.Instance.FeverOff();
        isGameEnded = true;
        GameEnd();
    }

    public void PausePanelOn()
    {
        Persistent_Stat Persistent_stat = StatManager.instance.Persistent_stat;
        Temporary_Stat Temporary_stat = StatManager.instance.Temporary_stat;
        StatMultiplier statMultiplier = StatManager.instance.statMultiplier;

        int days = TimeManager.instance.time / 86400;
        int hours = (TimeManager.instance.time % 86400) / 3600;
        int minutes = (TimeManager.instance.time % 3600) / 60;
        int seconds = TimeManager.instance.time % 60;

        AttackDamageStatValueText.text = ((Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) *
            (Persistent_stat.AttackDamageMultiplier * statMultiplier.AttackDamageMultiplier)).ToString("F1");

        CriticalDamageStatValueText.text = ((Persistent_stat.CriticalDamage + Temporary_stat.CriticalDamage) * 100).ToString("F1") + "%";

        CriticalChanceStatValueText.text = ((Persistent_stat.CriticalChance + Temporary_stat.CriticalChance) * 100).ToString("F1") + "%";

        EffectDamageStatValueText.text = ((Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) *
            (Persistent_stat.EffectDamageMultiplier * statMultiplier.EffectDamageMultiplier)).ToString("F1");

        EffectDurationStatValueText.text = (Temporary_stat.EffectDuration * 100).ToString("F1") + "%";

        GoldGainStatValueText.text = ((Persistent_stat.BonusGold + Temporary_stat.GoldGain) * 100).ToString("F1") + "%";

        ExpGainStatValueText.text = ((Persistent_stat.BonusExp + Temporary_stat.ExpGain) * 100).ToString("F1") + "%";

        NoticeLevelText_Pause.text = "Level " + LevelManager.Instance.Level.ToString();
        NoticePlaytimeText_Pause.text = "Playtime : " +
            (days > 0 ? string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", days, hours, minutes, seconds) :
            hours > 0 ? string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds) :
            string.Format("{0:D2}:{1:D2}", minutes, seconds));
        NoticePointText_Pause.text = "Point : " + ScoreManager.Instance.TotalScore.ToString();
        NoticeWaveText_Pause.text = "Wave : " + Wave.ToString();

        Time.timeScale = 0f;
    }

    public void PausePanelOff()
    {
        SoundManager.instance.SaveVolume();
        if (!LevelManager.Instance.LevelUpPanel.activeSelf)
            Time.timeScale = 1f;
    }

    private float[] stepTimes = new float[] { 0f, 3f, 4.5f, 6f, 7.5f, 9f };
    private int currentStep = 0;

    public void GameEnd()
    {
        RecordStatistics.Playtime += TimeManager.instance.time;
        bool isHighscore = false;
        LeaderboardManager.instance.RecordScore((long)ScoreManager.Instance.TotalScore, (x) => isHighscore = x);
        StatManager.instance.SavePersistentStats(); //for save gold
        AchievementManager.instance.AchievementCheck(RecordStatistics);
        StatManager.instance.UpdateStatistics(RecordStatistics);

        if (BallManager.Instance.InGameBall.Count > 0)
        {
            foreach (IBall ball in BallManager.Instance.InGameBall)
            {
                StartCoroutine(BallRemoveEffect(ball));
                BallManager.Instance.TakeBall(ball);
            }
        }

        PausePanel.SetActive(false);
        LevelManager.Instance.LevelUpPanel.SetActive(false);
        PausePanelOff();
        GameEndPanel.SetActive(true);

        NoticeLevelText_End.text = "Level  ";
        NoticeWaveText_End.text = "Wave : ";
        NoticePlaytimeText_End.text = $"Playtime : {00:D2}:{00:D2}";
        NoticeDestroyedBricksText_End.text = "Destroyed Bricks : ";
        NoticePointText_End.text = "Total Point : ";

        GameEndSequence = DOTween.Sequence().SetUpdate(true);
        Image panelImage = GameEndPanel.GetComponent<Image>();
        Color panelColor = panelImage.color;

        GameEndSequence.Append(
            DOVirtual.Float(0, 225f / 255f, 3f, value =>
            {
                Color newColor = panelColor;
                newColor.a = value;
                panelImage.color = newColor;
            })
        ).Insert(
            3f,
            DOVirtual.Float(0, LevelManager.Instance.Level, 1f, value =>
            {
                NoticeLevelText_End.text = "Level  " + Mathf.RoundToInt(value).ToString();
            })
        ).Insert(
            4.5f,
            DOVirtual.Float(0, Wave, 1f, value =>
            {
                NoticeWaveText_End.text = "Wave : " + Mathf.RoundToInt(value).ToString();
            })
        ).Insert(
            6f,
            DOVirtual.Float(0, TimeManager.instance.time, 1f, value =>
            {
                int totalSeconds = Mathf.RoundToInt(value);

                int days = totalSeconds / 86400;
                int hours = (totalSeconds % 86400) / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;

                if (days > 0)
                    NoticePlaytimeText_End.text = $"Playtime : {days:D2}:{hours:D2}:{minutes:D2}:{seconds:D2}";
                else if (hours > 0)
                    NoticePlaytimeText_End.text = $"Playtime : {hours:D2}:{minutes:D2}:{seconds:D2}";
                else
                    NoticePlaytimeText_End.text = $"Playtime : {minutes:D2}:{seconds:D2}";
            })
        ).Insert(
            7.5f,
            DOVirtual.Float(0, RecordStatistics.DestroyedBricks, 1f, value =>
            {
                NoticeDestroyedBricksText_End.text = "Destroyed Bricks : " + Mathf.RoundToInt(value).ToString();
            })
        ).Insert(
            9f,
            DOVirtual.Float(0, ScoreManager.Instance.TotalScore, 1f, value =>
            {
                NoticePointText_End.text = "Total Point : " + Mathf.RoundToInt(value).ToString();
            })
        );
        GameEndSequence.AppendCallback(() =>
        {
            if (isHighscore)
            {
                NoticeNewRecordText.gameObject.SetActive(true);
            }
        });
        GameEndSequence.Pause();
        GameEndSequence.Goto(0, true);
    }

    public void OnSkipGameEnd()
    {
        if (currentStep < stepTimes.Length - 1)
        {
            currentStep++;
            float nextTime = stepTimes[currentStep];
            GameEndSequence.Goto(nextTime, true);
        }
        else
        {
            GameEndSequence.Complete();
        }
    }

    private void LoadAds()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        InterstitialAd.Load(adUnitId, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              _interstitialAd = ad;
          });
        _interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            LoadAds();
            gameObject.GetComponent<SceneMove>().GoToMain();
        };
        _interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            gameObject.GetComponent<SceneMove>().GoToMain();
        };
    }

    public void ShowAds()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }


    private IEnumerator BallRemoveEffect(IBall ball)
    {
        VisualEffectManager.Instance.SpawnParticle("BallSacrifice", ball.transform.position, 30);
        yield return null;
    }
}
