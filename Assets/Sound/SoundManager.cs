using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] BgmClips;
    public AudioClip[] SfxClips;
    
    public AudioSource Bgm;
    Slider BgmVolumeSlider;
    TMP_Text BgmVolumeText;
    private int NowBgm;
    private Queue<int> BgmPlaylist;

    public AudioSource Sfx;
    Slider SfxVolumeSlider;
    TMP_Text SfxVolumeText;

    VolumeData volumeSaveData;
    private string VolumeData_filePath;
    void Awake()
    {
        instance = this;

        VolumeData_filePath = Application.persistentDataPath + "/PinballBrick_VolumeData.json";
        if (!File.Exists(VolumeData_filePath))
        {
            InitializeVolumeData();
        }

        Bgm = gameObject.transform.Find("Bgm").GetComponent<AudioSource>();
        Sfx = gameObject.transform.Find("Sfx").GetComponent<AudioSource>();
        BgmVolumeSlider = GameManager.instance.PausePanel.transform.Find("BgmText")
            .gameObject.transform.Find("BgmVolume").GetComponent<Slider>();
        SfxVolumeSlider = GameManager.instance.PausePanel.transform.Find("SfxText")
            .gameObject.transform.Find("SfxVolume").GetComponent<Slider>();
        BgmVolumeText = GameManager.instance.PausePanel.transform.Find("BgmText")
            .gameObject.transform.Find("BgmVolumeText").GetComponent<TMP_Text>();
        SfxVolumeText = GameManager.instance.PausePanel.transform.Find("SfxText")
            .gameObject.transform.Find("SfxVolumeText").GetComponent<TMP_Text>();

        volumeSaveData = LoadVolumeData();
        Bgm.volume = volumeSaveData.BgmVolume;
        BgmVolumeSlider.value = volumeSaveData.BgmVolume;
        Sfx.volume = volumeSaveData.SfxVolume;
        SfxVolumeSlider.value = volumeSaveData.SfxVolume;

        Bgm.mute = true;

        BgmPlaylist = new Queue<int>();
        List<int> X = new List<int>();
        for(int i = 0; i < BgmClips.Length; i++)
        {
            X.Add(i);
        }
        System.Random rng = new System.Random();
        List<int> shuffled = X.OrderBy(x => rng.Next()).ToList();
        for(int i = 0; i < shuffled.Count; i++)
        {
            BgmPlaylist.Enqueue(shuffled[i]);
        }
        Bgm.clip = null;
    }

    public void SaveVolume()
    {
        SaveVolumeData(volumeSaveData);
    }

    void Update()
    {
        if(GameManager.instance.PausePanel.activeSelf)
        {
            Bgm.volume = BgmVolumeSlider.value;
            volumeSaveData.BgmVolume = BgmVolumeSlider.value;

            Sfx.volume = SfxVolumeSlider.value;
            volumeSaveData.SfxVolume = SfxVolumeSlider.value;

            BgmVolumeText.text = ((int)(BgmVolumeSlider.value * 100)).ToString();
            SfxVolumeText.text = ((int)(SfxVolumeSlider.value * 100)).ToString();
        }
        if (!Bgm.isPlaying)
        {
            NowBgm = BgmPlaylist.Dequeue();
            Bgm.clip = BgmClips[NowBgm];
            BgmPlaylist.Enqueue(NowBgm);
            Bgm.Play();
        }
        if (LevelManager.Instance.LevelUpPanel.activeSelf)
        {
            Bgm.pitch = 0.7f;
        }
        else
        {
            Bgm.pitch = 1f;
        }
    }

    void InitializeVolumeData() // persistent stats
    {
        VolumeData VolumeData = new VolumeData
        {
            BgmVolume = 0.5f,
            SfxVolume = 0.5f
        };

        string json = JsonUtility.ToJson(VolumeData, true);
        File.WriteAllText(VolumeData_filePath, json);
    }

    public VolumeData LoadVolumeData()
    {
        string json = File.ReadAllText(VolumeData_filePath);
        return JsonUtility.FromJson<VolumeData>(json);
    }
    public void SaveVolumeData(VolumeData Volumedata)
    {
        string json = JsonUtility.ToJson(Volumedata, true);
        File.WriteAllText(VolumeData_filePath, json);

        json = File.ReadAllText(VolumeData_filePath);
        volumeSaveData = JsonUtility.FromJson<VolumeData>(json);
    }

    public void SpawnBrickSound(AudioSource audio)
    {
        audio.clip = SfxClips[0];
        audio.volume = SfxVolumeSlider.value;
        audio.Play();
    }

    public void BallAttackSound(AudioSource audio)
    {
        audio.clip = SfxClips[1];
        audio.volume = SfxVolumeSlider.value;
        audio.Play();
    }

    public void LevelUpSound()
    {
        Sfx.clip = SfxClips[2];
        Sfx.Play();
    }

    public void ButtonClickSound()
    {
        Sfx.clip = SfxClips[3];
        Sfx.Play();
    }
}
