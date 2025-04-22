using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject PausePanel;
    public TMP_Text WaveNoticeText;
    public int Wave;
    void Awake()
    {
        instance = this;
        PausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        WaveNoticeText = GameObject.Find("Canvas").transform.Find("WaveNoticeText").GetComponent<TMP_Text>();
        Wave = 0;
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; //DebugCanvas Off
    }

    public IEnumerator WaveNotice()
    {
        WaveNoticeText.text = "Wave " + Wave;
        WaveNoticeText.gameObject.SetActive(true);
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, time / 0.5f);
            Color color = WaveNoticeText.color;
            color.a = alpha;
            WaveNoticeText.color = color;
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
            yield return null;
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
        if(!LevelManager.Instance.LevelUpPanel.activeSelf)
            Time.timeScale = 1f;
    }
}
