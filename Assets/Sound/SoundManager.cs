using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using Redcode.Pools;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] BgmClips;
    public AudioClip[] SfxClips_UI;
    public AudioClip[] SfxClips_Game;
    public AudioClip[] SfxClips_Effect;

    public AudioSource Bgm;
    Slider BgmVolumeSlider;
    TMP_Text BgmVolumeText;
    private int NowBgm;
    private Queue<int> BgmPlaylist;

    public AudioSource Sfx;
    Slider SfxVolumeSlider;
    TMP_Text SfxVolumeText;

    public PoolManager Pool;


    VolumeData volumeSaveData;
    private string VolumeData_filePath;
    void Awake()
    {
        instance = this;
        Pool = GetComponent<PoolManager>();
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

    public void PausePausableSounds()
    {
        foreach(PausableSoundObject Obj in Pool.GetComponentsInChildren<PausableSoundObject>())
        {
            Obj.GetComponent<AudioSource>().Pause();
        }
    }

    public void UnPausePausableSounds()
    {
        foreach (PausableSoundObject Obj in Pool.GetComponentsInChildren<PausableSoundObject>())
        {
            Obj.GetComponent<AudioSource>().UnPause();
        }
    }
    private IEnumerator WaitForAudioFinish(AudioSource source, float Duration)
    {
        yield return new WaitForSeconds(Duration);
        Pool.TakeToPool<SoundObject>(source.GetComponent<SoundObject>());
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
        audio.volume = SfxVolumeSlider.value / 2f;
        audio.PlayOneShot(SfxClips_Game[0]);
    }

    public void BallAttackSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value / 3f;
        audio.PlayOneShot(SfxClips_Game[1]);
    }

    public void LevelUpSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value / 1.25f;
        audio.PlayOneShot(SfxClips_Game[2]);
    }

    public void ButtonClickSound_Light()
    {
        Sfx.clip = SfxClips_UI[0];
        Sfx.Play();
    }

    public void AugmentDrawSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(SfxClips_Game[3]);
    }

    public void HighRarityAugmentDrawSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(SfxClips_Game[4]);
    }

    public void AugmentFlashSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(SfxClips_Game[5]);
    }

    public void AugmentFlashSound2(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(SfxClips_Game[6]);
    }

    public void AugmentShakeSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(SfxClips_Game[7]);
    }

    public void AugmentSelectSound()
    {
        Sfx.clip = SfxClips_Game[8];
        Sfx.Play();
    }

    public void AugmentClickSound()
    {
        Sfx.clip = SfxClips_Game[9];
        Sfx.Play();
    }

    public void GetGoldSOund(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value / 1.1f;
        audio.PlayOneShot(SfxClips_Game[10]);
    }
    public void GetExpSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value / 1.1f;
        audio.PlayOneShot(SfxClips_Game[11]);
    }

    public void HolyWaterSound1()
    {
        AudioClip clip = SfxClips_Effect[0];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void HolyWaterSound2()
    {
        AudioClip clip = SfxClips_Effect[1];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void SatelliteBeamSound(float Duration)
    {
        AudioClip clip = SfxClips_Effect[2];
        AudioSource audio = Pool.GetFromPool<PausableSoundObject>().GetComponent<AudioSource>();
        audio.clip = clip;
        audio.loop = true;
        audio.volume = SfxVolumeSlider.value;
        audio.Play();
        StartCoroutine(WaitForAudioFinish(audio, Duration));
    }

    public void SatelliteBeamChargeSound()
    {
        AudioClip clip = SfxClips_Effect[3];
        AudioSource audio = Pool.GetFromPool<PausableSoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void StartFireSound()
    {
        AudioClip clip = SfxClips_Effect[4];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void FireMaxStackSound()
    {
        AudioClip clip = SfxClips_Effect[5];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void ExplosionSound_Big()
    {
        AudioClip clip = SfxClips_Effect[6];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void ExplosionSound_Small()
    {
        AudioClip clip = SfxClips_Effect[7];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void AdditionalAttackSound()
    {
        AudioClip clip = SfxClips_Effect[8];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void ExecutionSound()
    {
        AudioClip clip = SfxClips_Effect[9];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void StrongAdditionalAttackSound()
    {
        AudioClip clip = SfxClips_Effect[10];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void PoisonSound()
    {
        AudioClip clip = SfxClips_Effect[11];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value * 1.25f;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void SpreadPoisonSound()
    {
        AudioClip clip = SfxClips_Effect[12];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value / 1.25f;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }
}
