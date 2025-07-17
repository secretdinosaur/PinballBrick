using UnityEngine;
using TMPro;
using System.Collections;
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    Coroutine TimeRoutine;
    public int time;

    private float deltaTime = 0f;

    private void Awake()
    {
        instance = this;
        TimeRoutine = null;

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
}
