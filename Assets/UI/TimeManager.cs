using UnityEngine;
using TMPro;
using System.Collections;
public class TimeManager : MonoBehaviour
{
    TMP_Text TimeText;
    Coroutine TimeRoutine;
    public int time;

    private float deltaTime = 0f;

    private void Awake()
    {
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
            yield return new WaitForSeconds(1f);
            time++;
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
