using UnityEngine;
using TMPro;
using System.Collections;
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    TMP_Text TimeText;
    Coroutine TimeRoutine;
    public int time;

    private float deltaTime = 0f;

    private void Awake()
    {
        instance = this;
        TimeRoutine = null;
        TimeText = GameObject.Find("Canvas").transform.Find("GameTimeText").GetComponent<TMP_Text>();

        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        TimeRoutine = StartCoroutine(IncreaseTime());
    }

    IEnumerator IncreaseTime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            time++;

            int days = time / 86400;
            int hours = (time % 86400) / 3600;
            int minutes = (time % 3600) / 60;
            int seconds = time % 60;

            GameManager.instance.NoticePlaytimeText.text = "Playtime : " +
            (days > 0 ? string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", days, hours, minutes, seconds) :
            hours > 0 ? string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds) :
            string.Format("{0:D2}:{1:D2}", minutes, seconds));
        }
    }

    private void Update()
    {
        if (TimeRoutine != null)
        {
            //TimeText.text = time / 60 + ":" + time % 60;

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            TimeText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}
