using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private Vector2 initialPosition;
    void Start()
    {
        instance = this;
        initialPosition = transform.position;
        AdjustCamera();
    }
    void AdjustCamera()
    {
        float targetAspect = 9f / 19.5f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            Rect rect = camera.rect;

            float scaleWidth = 1.0f / scaleHeight;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    private float Timer;
    public void ShakeCamera(float timer = 0.3f)
    {
        Timer = timer;
        StartCoroutine(GetShakeCamera());
    }
    IEnumerator GetShakeCamera()
    {
        while (Timer > 0)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * 0.1f;
            transform.position = new Vector3(initialPosition.x + shakeOffset.x, initialPosition.y + shakeOffset.y);
            Timer -= Time.deltaTime;
            yield return null;
        }
        transform.position = initialPosition;
    }
}
