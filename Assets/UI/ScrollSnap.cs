using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollSnap : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollRect;
    public RectTransform content;

    public int itemCount = 3; 
    public float itemSize = 1440;

    private float targetPosition;
    float Startpos;
    float Endpos;
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
        if (Startpos > Endpos)
        {
            targetPosition = -(CurrentIndex + 1) * itemSize - 720;
        }
        else if(Startpos < Endpos)
        {
            targetPosition = -(CurrentIndex - 1) * itemSize - 720;
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
}
