using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject PausePanel;
    public TMP_Text WaveNoticeText;
    public TMP_Text WaveExtraNoticeText;
    public int Wave;
    void Awake()
    {
        instance = this;
        PausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
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
        Time.timeScale = 0f;
    }

    public void PausePanelOff()
    {
        SoundManager.instance.SaveVolume();
        if (!LevelManager.Instance.LevelUpPanel.activeSelf)
            Time.timeScale = 1f;
    }
}
