using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject PausePanel;

    TMP_Text AttackDamageStatValueText;
    TMP_Text CriticalDamageStatValueText;
    TMP_Text CriticalChanceStatValueText;
    TMP_Text EffectDamageStatValueText;
    TMP_Text EffectCriticalDamageStatValueText;
    TMP_Text EffectDurationStatValueText;
    TMP_Text GoldGainStatValueText;
    TMP_Text ExpGainStatValueText;

    TMP_Text NoticeLevelText;
    public TMP_Text NoticePlaytimeText;
    TMP_Text NoticePointText;
    TMP_Text NoticeWaveText;

    public TMP_Text WaveNoticeText;
    public TMP_Text WaveExtraNoticeText;

    public int Wave;
    void Awake()
    {
        instance = this;
        PausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;

        AttackDamageStatValueText = PausePanel.transform.Find("Stats/AttackDamageIcon/AttackDamageStatValueText").GetComponent<TMP_Text>();
        CriticalDamageStatValueText = PausePanel.transform.Find("Stats/CriticalDamageIcon/CriticalDamageStatValueText").GetComponent<TMP_Text>();
        CriticalChanceStatValueText = PausePanel.transform.Find("Stats/CriticalChanceIcon/CriticalChanceStatValueText").GetComponent<TMP_Text>();
        EffectDamageStatValueText = PausePanel.transform.Find("Stats/EffectDamageIcon/EffectDamageStatValueText").GetComponent<TMP_Text>();
        EffectCriticalDamageStatValueText = PausePanel.transform.Find("Stats/EffectCriticalDamageIcon/EffectCriticalDamageStatValueText").GetComponent<TMP_Text>();
        EffectDurationStatValueText = PausePanel.transform.Find("Stats/EffectDurationIcon/EffectDurationStatValueText").GetComponent<TMP_Text>();
        GoldGainStatValueText = PausePanel.transform.Find("Stats/GoldGainIcon/GoldGainStatValueText").GetComponent<TMP_Text>();
        ExpGainStatValueText = PausePanel.transform.Find("Stats/ExpGainIcon/ExpGainStatValueText").GetComponent<TMP_Text>();

        NoticeLevelText = PausePanel.transform.Find("Level").GetComponent<TMP_Text>();
        NoticePlaytimeText = PausePanel.transform.Find("Playtime").GetComponent<TMP_Text>();
        NoticePointText = PausePanel.transform.Find("Point").GetComponent<TMP_Text>();
        NoticeWaveText = PausePanel.transform.Find("Wave").GetComponent<TMP_Text>();

        WaveNoticeText = GameObject.Find("Canvas").transform.Find("WaveNoticeText").GetComponent<TMP_Text>();
        WaveExtraNoticeText = GameObject.Find("Canvas").transform.Find("WaveExtraNoticeText").GetComponent<TMP_Text>();
        Wave = 0;
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; //DebugCanvas Off
    }

    public IEnumerator WaveNotice()
    {
        if(Wave % 10 != 0)
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
        WaveNoticeText.gameObject.SetActive(false);
    }

    public void GameExit()
    {
        SoundManager.instance.SaveVolume();

        Application.Quit(); //추후에 결과창으로 이동하는걸로 변경, 결과창에서 메인으로 돌아갈 수 있게끔.
    }

    public void PausePanelOn()
    {
        Persistent_Stat Persistent_stat = StatManager.instance.Persistent_stat;
        Temporary_Stat Temporary_stat = StatManager.instance.Temporary_stat;
        StatMultiplier statMultiplier = StatManager.instance.statMultiplier;

        AttackDamageStatValueText.text = ((Persistent_stat.AttackDamage + Temporary_stat.AttackDamage) *
            (Persistent_stat.AttackDamageMultiplier * statMultiplier.AttackDamageMultiplier)).ToString();

        CriticalDamageStatValueText.text = ((Persistent_stat.CriticalDamage + Temporary_stat.CriticalDamage) * 100).ToString() + "%";

        CriticalChanceStatValueText.text = ((Persistent_stat.CriticalChance + Temporary_stat.CriticalChance) * 100).ToString() + "%";

        EffectDamageStatValueText.text = ((Persistent_stat.EffectDamage + Temporary_stat.EffectDamage) *
            (Persistent_stat.EffectDamageMultiplier * statMultiplier.EffectDamageMultiplier)).ToString();

        EffectCriticalDamageStatValueText.text = (Temporary_stat.EffectCriticalDamage * 100).ToString() + "%";

        EffectDurationStatValueText.text = Temporary_stat.EffectDuration.ToString() + "s";

        GoldGainStatValueText.text = ((Persistent_stat.BonusGold + Temporary_stat.GoldGain) * 100).ToString() + "%";

        ExpGainStatValueText.text = ((Persistent_stat.BonusExp + Temporary_stat.ExpGain) * 100).ToString() + "%";

        NoticeLevelText.text = "Level " + LevelManager.Instance.Level.ToString();
        //NoticetimeText is in the TimeManager
        NoticePointText.text = "Point : " + ScoreManager.Instance.TotalScore.ToString();
        NoticeWaveText.text = "Wave : " + Wave.ToString();

        Time.timeScale = 0f;
    }

    public void PausePanelOff()
    {
        SoundManager.instance.SaveVolume();
        if (!LevelManager.Instance.LevelUpPanel.activeSelf)
            Time.timeScale = 1f;
    }
}
