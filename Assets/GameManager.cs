using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject PausePanel;
    void Awake()
    {
        instance = this;
        PausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; //DebugCanvas Off
    }
}
