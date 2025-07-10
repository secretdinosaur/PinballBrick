using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollMove : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;

    public int itemCount = 3; 
    public float itemSize = 1440;

    private float targetPosition;
    float Startpos;
    float Endpos;

    public GameObject[] UnderBarButtons;
    public float MinimumScrollValue;
    void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Startpos = content.anchoredPosition.x;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        float Endpos = content.anchoredPosition.x;
        int CurrentIndex = (int)(-(Startpos + 720) / itemSize);
        if (Startpos > Endpos + MinimumScrollValue)
        {
            targetPosition = -(CurrentIndex + 1) * itemSize - 720;
            CurrentIndex += 1;
        }
        else if(Startpos < Endpos - MinimumScrollValue)
        {
            targetPosition = -(CurrentIndex - 1) * itemSize - 720;
            CurrentIndex -= 1;
        }
        for (int j = 0; j < 5; j++)
        {
            if (j != CurrentIndex)
            {
                UnderBarButtons[j].GetComponent<Image>().material.SetFloat("_Alpha", 1.5f);
                UnderBarButtons[j].transform.localScale = Vector3.one * 1f;
            }
            else
            {
                UnderBarButtons[j].GetComponent<Image>().material.SetFloat("_Alpha", 2.5f);
                UnderBarButtons[j].transform.localScale = Vector3.one * 1.1f;
            }
        }
        StopAllCoroutines();
        StartCoroutine(SmoothMove(content.anchoredPosition.x, targetPosition, 0.2f));
    }

    System.Collections.IEnumerator SmoothMove(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newX = Mathf.Lerp(start, end, elapsed / duration);
            content.anchoredPosition = new Vector2(newX, content.anchoredPosition.y);
            yield return null;
        }
        content.anchoredPosition = new Vector2(end, content.anchoredPosition.y);
    }

    public void OnUnderBarButtonClick(int i)
    {
        content.anchoredPosition = new Vector2(-i * itemSize - itemSize / 2, content.anchoredPosition.y);
        for(int j = 0; j < 5; j++)
        {
            if(j != i)
            {
                UnderBarButtons[j].GetComponent<Image>().material.SetFloat("_Alpha", 1.5f);
                UnderBarButtons[j].transform.localScale = Vector3.one * 1f;
            }
            else
            {
                UnderBarButtons[j].GetComponent<Image>().material.SetFloat("_Alpha", 2.5f);
                UnderBarButtons[j].transform.localScale = Vector3.one * 1.1f;
            }
        }
    }
}
