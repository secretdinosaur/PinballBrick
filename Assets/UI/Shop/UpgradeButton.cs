using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;

public class UpgradeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Coroutine repeatCoroutine;
    private bool isPointerDown = false;
    public float holdDelay = 1f;
    public float repeatRate = 0.1f;
    private float holdTimer = 0f;
    public int pressedNum;
    void Update()
    {
        if (isPointerDown)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDelay && repeatCoroutine == null)
            {
                repeatCoroutine = StartCoroutine(UpgradeManager.Instance.RepeatPurchace(pressedNum));
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        holdTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        holdTimer = 0f;

        if (repeatCoroutine != null)
        {
            StopCoroutine(repeatCoroutine);
            repeatCoroutine = null;
        }
    }
}
