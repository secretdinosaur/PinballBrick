using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] BgmClips;
    public AudioClip[] SfxClips;
    
    AudioSource Bgm;
    Slider BgmVolume;
    TMP_Text BgmVolumeText;
    private int NowBgm;
    private Queue<int> BgmPlaylist;

    AudioSource Sfx;
    Slider SfxVolume;
    TMP_Text SfxVolumeText;
    void Awake()
    {
        instance = this;
        Bgm = gameObject.transform.Find("Bgm").GetComponent<AudioSource>();
        Sfx = gameObject.transform.Find("Sfx").GetComponent<AudioSource>();
        BgmVolume = GameManager.instance.PausePanel.transform.Find("BgmText")
            .gameObject.transform.Find("BgmVolume").GetComponent<Slider>();
        SfxVolume = GameManager.instance.PausePanel.transform.Find("SfxText")
            .gameObject.transform.Find("SfxVolume").GetComponent<Slider>();
        BgmVolumeText = GameManager.instance.PausePanel.transform.Find("BgmText")
            .gameObject.transform.Find("BgmVolumeText").GetComponent<TMP_Text>();
        SfxVolumeText = GameManager.instance.PausePanel.transform.Find("SfxText")
            .gameObject.transform.Find("SfxVolumeText").GetComponent<TMP_Text>();

        Bgm.volume = 0.5f;
        Sfx.volume = 0.5f;

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

    public void Update()
    {
        if(GameManager.instance.PausePanel.activeSelf)
        {
            Bgm.volume = BgmVolume.value;
            Sfx.volume = SfxVolume.value;
            BgmVolumeText.text = ((int)(BgmVolume.value * 100)).ToString();
            SfxVolumeText.text = ((int)(SfxVolume.value * 100)).ToString();
        } //게임 껏다켜도 볼륨 유지되게 변경하기
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

    public void SpawnBrickSound(AudioSource audio)
    {
        audio.clip = SfxClips[0];
        audio.volume = SfxVolume.value;
        audio.Play();
    }

    public void BallAttackSound(AudioSource audio)
    {
        audio.clip = SfxClips[1];
        audio.volume = SfxVolume.value;
        audio.Play();
    }

    public void LevelUpSound()
    {
        Sfx.clip = SfxClips[2];
        Sfx.Play();
    }
}
