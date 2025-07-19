using DG.Tweening;
using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private static bool hasRun = false;
    Sequence GameOpeningSequence;
    GameObject GameOpeningPanel;
    Image Logo;
    private void Awake()
    {
        if (!hasRun)
        {
            GameOpeningPanel = GameObject.Find("Canvas").transform.Find("GameOpeningPanel").gameObject;
            Logo = GameOpeningPanel.transform.Find("Logo").GetComponent<Image>();
            Opening();
            hasRun = true;
        }
    }
    private float[] stepTimes = new float[] { 0f, 3f, 7.5f };
    private int currentStep = 0;
    private void Opening()
    {
        GameOpeningSequence = DOTween.Sequence().SetUpdate(false);

        Image panelImage = GameOpeningPanel.GetComponent<Image>();
        Color panelColor = panelImage.color;
        Color logoColor = Logo.color;
        logoColor.a = 0f;
        Logo.color = logoColor;

        panelColor.a = 1f;
        panelImage.color = panelColor;

        GameOpeningPanel.SetActive(true);
        GameOpeningSequence.Append(
            DOVirtual.Float(0, 1f, 3f, value =>
            {
                Color newColor = logoColor;
                newColor.a = value;
                Logo.color = newColor;
            })
        ).Insert(
            4.5f,
            DOVirtual.Float(1, 0f, 3f, value =>
            {
                Color newColor = panelColor;
                newColor.a = value;
                panelImage.color = newColor;

                Color newColor1 = logoColor;
                newColor1.a = value;
                Logo.color = newColor1;
            })
        );
        GameOpeningSequence.AppendCallback(() =>
        {
            GameOpeningPanel.SetActive(false);
        });
        GameOpeningSequence.Pause();
        GameOpeningSequence.Goto(0, true);
    }
    public void OnSkipGameOpen()
    {
        if (currentStep < stepTimes.Length - 1)
        {
            currentStep++;
            float nextTime = stepTimes[currentStep];
            GameOpeningSequence.Goto(nextTime, true);
        }
        else
        {
            GameOpeningSequence.Complete();
        }
    }
}
