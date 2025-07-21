using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using Redcode.Pools;
using System.Collections;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] BgmClips;
    public AudioClip[] SfxClips_UI;
    public AudioClip[] SfxClips_Game;
    public AudioClip[] SfxClips_Effect;

    public AudioSource Bgm;
    public Slider BgmVolumeSlider;
    public TMP_Text BgmVolumeText;
    private int NowBgm;
    private Queue<int> BgmPlaylist;

    public AudioSource Sfx;
    public Slider SfxVolumeSlider;
    public TMP_Text SfxVolumeText;

    public PoolManager Pool;

    public GameObject ArcLinkSoundObj;

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

        if (SceneManager.GetActiveScene().name != "Main")
        {
            BgmVolumeSlider = GameManager.instance.PausePanel.transform.Find("BgmText")
            .gameObject.transform.Find("BgmVolume").GetComponent<Slider>();
            SfxVolumeSlider = GameManager.instance.PausePanel.transform.Find("SfxText")
                .gameObject.transform.Find("SfxVolume").GetComponent<Slider>();
            BgmVolumeText = GameManager.instance.PausePanel.transform.Find("BgmText")
                .gameObject.transform.Find("BgmVolumeText").GetComponent<TMP_Text>();
            SfxVolumeText = GameManager.instance.PausePanel.transform.Find("SfxText")
                .gameObject.transform.Find("SfxVolumeText").GetComponent<TMP_Text>();
        }
        

        volumeSaveData = LoadVolumeData();
        Bgm.volume = volumeSaveData.BgmVolume;
        Sfx.volume = volumeSaveData.SfxVolume;

        SfxVolumeSlider.value = volumeSaveData.SfxVolume;
        BgmVolumeSlider.value = volumeSaveData.BgmVolume;

        

        Bgm.mute = true; //이거 뮤트가 아니라 멈췄다가 실행되게 하기 GameManager에 StartGame 코루틴이랑 연동해서 사용

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
        if (source.gameObject.activeSelf)
            Pool.TakeToPool<SoundObject>(source.GetComponent<SoundObject>());
    }

    private IEnumerator WaitForAudioFinish_Pausable(AudioSource source, float Duration)
    {
        yield return new WaitForSeconds(Duration);
        if (source.gameObject.activeSelf)
            Pool.TakeToPool <PausableSoundObject>(source.GetComponent<PausableSoundObject>());
    }

    public void TakeSound(AudioSource source)
    {
        if(source.gameObject.activeSelf)
            Pool.TakeToPool<SoundObject>(source.GetComponent<SoundObject>());
    }

    public void SaveVolume()
    {
        SaveVolumeData(volumeSaveData);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main" && GameManager.instance.PausePanel.activeSelf)
        {
            Bgm.volume = BgmVolumeSlider.value;
            volumeSaveData.BgmVolume = BgmVolumeSlider.value;

            Sfx.volume = SfxVolumeSlider.value;
            volumeSaveData.SfxVolume = SfxVolumeSlider.value;

            BgmVolumeText.text = ((int)(BgmVolumeSlider.value * 100)).ToString();
            SfxVolumeText.text = ((int)(SfxVolumeSlider.value * 100)).ToString();
        }
        else if(SceneManager.GetActiveScene().name == "Main")
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
            if (SceneManager.GetActiveScene().name == "Main")
            {
                Bgm.clip = BgmClips[3];
                Bgm.Play();
            }
            else
            {
                NowBgm = BgmPlaylist.Dequeue();
                Bgm.clip = BgmClips[NowBgm];
                BgmPlaylist.Enqueue(NowBgm);
                Bgm.Play();
            }
            
        }
        if (SceneManager.GetActiveScene().name != "Main" && LevelManager.Instance.LevelUpPanel.activeSelf)
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

    public void ButtonClickSound_Light()
    {
        AudioClip clip = SfxClips_UI[0];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = SfxVolumeSlider.value;
        audio.Play();
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void MissionCompleteSound()
    {
        AudioClip clip = SfxClips_UI[1];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = SfxVolumeSlider.value;
        audio.Play();
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void UpgradeSound()
    {
        AudioClip clip = SfxClips_UI[2];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = SfxVolumeSlider.value;
        audio.Play();
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void SpawnBrickSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value / 2f;
        audio.pitch = Random.Range(0.9f, 1.1f);
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
        if (SfxVolumeSlider != null)
            audio.volume = SfxVolumeSlider.value / 1.1f;
        else
            audio.volume = 0.5f;
        audio.PlayOneShot(SfxClips_Game[10]);
    }
    public void GetExpSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value / 1.1f;
        audio.PlayOneShot(SfxClips_Game[11]);
    }

    public void BallSpawnSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.PlayOneShot(SfxClips_Game[12]);
    }

    public void SpawnBossSound(AudioSource audio)
    {
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(SfxClips_Game[13]);
    }


    public void BrickDieSound(string BrickType)
    {
        AudioClip clip = SfxClips_Game[14];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        switch(BrickType)
        {
            case "HardBrick":
            case "IgnoreBrick":
                audio.pitch = Random.Range(0.5f, 0.6f);
                break;
            case "Brick":
            case "TwinBrick":
                audio.pitch = Random.Range(0.9f, 1.1f);
                break;
            case "HealBrick":
            case "TeleportBrick":
            case "AbsorbBrick":
                audio.pitch = Random.Range(1.2f, 1.3f);
                break;
            
            case "SlimeBrick":
                audio.pitch = Random.Range(1.5f, 1.6f);
                break;
            case "GoldBrick":
            case "ExpBrick":
            case "BallBrick":
                audio.pitch = Random.Range(2f, 2.1f);
                break;
        }
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
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
        StartCoroutine(WaitForAudioFinish_Pausable(audio, Duration));
    }

    public void SatelliteBeamChargeSound()
    {
        AudioClip clip = SfxClips_Effect[3];
        AudioSource audio = Pool.GetFromPool<PausableSoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish_Pausable(audio, clip.length));
    }

    public void StartFireSound()
    {
        AudioClip clip = SfxClips_Effect[4];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));

        AudioClip clip2 = SfxClips_Effect[16];
        AudioSource audio2 = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio2.volume = SfxVolumeSlider.value;
        audio2.PlayOneShot(clip2);
        StartCoroutine(WaitForAudioFinish(audio2, clip2.length));
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
        audio.volume = SfxVolumeSlider.value * 2f;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void MaxStackPoisonSound()
    {
        AudioClip clip = SfxClips_Effect[11];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value * 2f;
        audio.pitch = 1.5f;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void SpreadPoisonSound()
    {
        AudioClip clip = SfxClips_Effect[12];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void CurseSound(float pitch)
    {
        AudioClip clip = SfxClips_Effect[13];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.pitch = 1 + pitch / 5;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public void LightningSound()
    {
        AudioClip clip = SfxClips_Effect[14];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value;
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }

    public AudioSource ArcLinkSound(float Duration)
    {
        AudioClip clip = SfxClips_Effect[15];
        AudioSource audio = ArcLinkSoundObj.GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = SfxVolumeSlider.value;

        return audio;
    }

    public void GameOverSound()
    {
        AudioClip clip = SfxClips_Game[15];
        AudioSource audio = Pool.GetFromPool<SoundObject>().GetComponent<AudioSource>();
        audio.volume = SfxVolumeSlider.value * 1.75f;
        audio.PlayOneShot(clip);
        StartCoroutine(WaitForAudioFinish(audio, clip.length));
    }
}
