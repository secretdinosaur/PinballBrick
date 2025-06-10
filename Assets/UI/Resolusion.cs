using UnityEngine;
using UnityEngine.UI;
public class Resolusion : MonoBehaviour
{
    public CanvasScaler cs;
    void Start()
    {
        float fixedAspectRatio = 0.5f;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;
        if (currentAspectRatio > fixedAspectRatio) cs.matchWidthOrHeight = 1;
        else if (currentAspectRatio < fixedAspectRatio) cs.matchWidthOrHeight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
