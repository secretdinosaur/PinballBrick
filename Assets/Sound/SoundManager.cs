using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] BgmClips;
    public AudioClip[] SfxClips;
    AudioSource Bgm;
    private int NowBgm;
    private Queue<int> BgmPlaylist;
    AudioSource Sfx;
    void Awake()
    {
        instance = this;
        Bgm = gameObject.transform.Find("Bgm").GetComponent<AudioSource>();
        Sfx = gameObject.transform.Find("Sfx").GetComponent<AudioSource>();

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
        audio.Play();
    }

    public void BallAttackSound(AudioSource audio)
    {
        audio.clip = SfxClips[1];
        audio.Play();
    }

    public void LevelUpSound()
    {
        Sfx.clip = SfxClips[2];
        Sfx.Play();
    }
}
