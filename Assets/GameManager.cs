using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject PausePanel;
    public int Wave;
    void Awake()
    {
        instance = this;
        PausePanel = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        Wave = 0;
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; //DebugCanvas Off
    }

    public void GameExit()
    {
        SoundManager.instance.SaveVolume();

        Application.Quit(); //추후에 결과창으로 이동하는걸로 변경, 결과창에서 메인으로 돌아갈 수 있게끔.
    }

    public void PausePanelOn()
    {
        Time.timeScale = 0f;
    }

    public void PausePanelOff()
    {
        if(!LevelManager.Instance.LevelUpPanel.activeSelf)
            Time.timeScale = 1f;
    }
}
