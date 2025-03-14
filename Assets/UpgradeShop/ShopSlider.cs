using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ShopSlider : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    private float[] positions = { 0f, 0.5f, 1f};
    private int currentPage = 0;
    private bool isScrolling = false;

    public void NextPage()
    {
        if (isScrolling) return;

        if (currentPage < positions.Length - 1)
        {
            currentPage++;
            StartCoroutine(SmoothScroll(positions[currentPage]));

        }
        else
        {
            scrollRect.horizontalNormalizedPosition = 0f;
            currentPage = 1;
            StartCoroutine(SmoothScroll(positions[currentPage]));
        }
    }

    public void PrevPage()
    {
        if (isScrolling) return;

        if (currentPage > 0)
        {
            currentPage--;
            StartCoroutine(SmoothScroll(positions[currentPage]));

        }
        else
        {
            scrollRect.horizontalNormalizedPosition = 1f;
            currentPage = 1;
            StartCoroutine(SmoothScroll(positions[currentPage]));
        }
    }

    private IEnumerator SmoothScroll(float target)
    {
        isScrolling = true;
        float duration = 0.2f;
        float elapsed = 0f;
        float start = scrollRect.horizontalNormalizedPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            scrollRect.horizontalNormalizedPosition = Mathf.SmoothStep(start, target, elapsed / duration);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = target;
        isScrolling = false;
    }
}