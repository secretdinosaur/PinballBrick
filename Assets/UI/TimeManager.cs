using UnityEngine;
using TMPro;
using System.Collections;
public class TimeManager : MonoBehaviour
{
    TMP_Text TimeText;
    Coroutine TimeRoutine;
    public int time;

    private void Awake()
    {
        TimeRoutine = null;
        TimeText = GameObject.Find("Canvas").transform.Find("GameTimeText").GetComponent<TMP_Text>();
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
            TimeText.text = time / 60 + ":" + time % 60;
        }
    }
}
