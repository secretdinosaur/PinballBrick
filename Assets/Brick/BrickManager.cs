using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using System.Drawing;
using NUnit.Framework;
public class BrickManager : MonoBehaviour
{
    public List<float> HPPool;

    public static BrickManager Instance;
    public List<IBrick> CurrentBricks = new List<IBrick>();
    private PoolManager PoolManager;

    private float ScreenWidth;
    private float ScreenHeight;

    private float HorizontalGridSize = 6;
    private float VerticalGridSize = 10;

    public BrickInfo BrickInfo;

    public List<Vector2> availablePositions = new List<Vector2>();
    public List<Vector2> usingPositions = new List<Vector2>();

    public IBrick MainBrick;
    public IBrick BigBossBrick;

    public Coroutine SpawnBrickRoutine;

    Queue<Queue<IBrick>> SpawnBrickQueue;
    public Queue<IBrick> BrickQueue;

    public float SpecialBrickSpawnChance;
    public float SpecialBrickSpawnChanceValue;

    public bool isRapid;
    public float RapidStageChance;
    public float RapidStageChanceValue;

    public float BaseBrickSpawnValue;

    public float BaseBrickHealthValue;
    public float BaseBossHealthValue;

    public float BaseSpawnTimeValue;
    public int BaseWaitingTimeValue;

    public int BaseBossSpawnValue;
    public int NextBossSpawnWave;
    public int BossSpawnWaveNumber;

    private bool WavePause;
    public Coroutine WaveRoutine;

    enum BrickType
    {
        Brick,
        HealBrick,
        HardBrick,
        SlimeBrick,
        ShieldBrick,
        TeleportBrick,
        IgnoreBrick,
        AbsorbBrick,
        TwinBrick,
        RebornBrick,
    }

    public List<float> BrickExpValue = new List<float>
    {
        10f,
        12.5f,
        20f,
        10f,
        20f,
        15f,
        20f,
        20f,
        20f,
        13.5f,
    };

    void Awake()
    {
        Instance = this;
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        InitializeGrid();
        InitializeVariables();
        SpawnBrickQueue = new Queue<Queue<IBrick>>();
        BrickQueue = new Queue<IBrick>();
        PoolManager = GetComponent<PoolManager>();
        SpawnBrickRoutine = null;
    }

    private void InitializeVariables()
    {
        SpecialBrickSpawnChance = 0;
        SpecialBrickSpawnChanceValue = 1f;
        isRapid = false;

        RapidStageChance = 0;
        RapidStageChanceValue = 0.09f;
        BaseWaitingTimeValue = 5;
        BaseSpawnTimeValue = 1f;

        BaseBrickSpawnValue = 1f;
        BaseBrickHealthValue = 1f;

        BaseBossHealthValue = 1f;
        BaseBossSpawnValue = 1;
        NextBossSpawnWave = 10;
        BossSpawnWaveNumber = 10;

        WavePause = false;
    }

    public void UpdateCurrentBricks(IBrick brick)
    {
        CurrentBricks.RemoveAll(item => item == null);
        if (!CurrentBricks.Contains(brick))
        {
            CurrentBricks.Add(brick);
        }
    }
    void InitializeGrid()
    {
        float cellWidth = 4.75f / HorizontalGridSize;
        float cellHeight = 6f / VerticalGridSize;
        availablePositions = new List<Vector2>();
        for (int x = 0; x < HorizontalGridSize; x++)
        {
            for (int y = 0; y < VerticalGridSize; y++)
            {
                float posX = -2.375f+ x * cellWidth + cellWidth / 2;
                float posY = -1.75f + y * cellHeight + cellHeight / 2;
                availablePositions.Add(new Vector2(posX, posY));
            }
        }
    }

    public Vector2 GenerateRandomPosition()
    {
        int randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
        Vector2 selectedPosition = availablePositions[randomIndex];

        usingPositions.Add(selectedPosition);
        availablePositions.RemoveAt(randomIndex);
        return selectedPosition;
    }

    public void TeleportBrickPosition(IBrick brick)
    {
        availablePositions.Add(brick.transform.position);
        usingPositions.Remove(brick.transform.position);
        Vector2 RandomPosition = GenerateRandomPosition();
        brick.transform.position = new Vector3(RandomPosition.x, RandomPosition.y, 35);
    }
    public void TakeBrick(IBrick brick)
    {
        if (brick.IsBoss)
        {
            WavePause = false;

            if (AugmentManager.instance.determineAugment(typeof(DoubleBoss)))
            {
                int BossSpawned = 2;
                for (int i = 0; i < CurrentBricks.Count; i++)
                {
                    if (CurrentBricks[i].IsBoss)
                        BossSpawned -= 1;
                }
                if (BossSpawned == 1) // currentbrick does not updated yet
                    WavePause = false;
                else
                    WavePause = true;
            }
        }
        SoundManager.instance.BrickDieSound(brick.BrickType);
        if (brick.gameObject.activeSelf)
        {
            if (brick.Fire != null)
            {
                brick.StopCoroutine(brick.Fire);
                brick.Fire = null;
                brick.lineRenderer.enabled = false;
            }
            if (brick.Poison != null)
            {
                brick.StopCoroutine(brick.Poison);
                brick.Poison = null;
                brick.PoisonEffect.SetActive(false);
            }
            if (brick.Decay != null)
            {
                brick.StopCoroutine(brick.Decay);
                brick.Decay = null;
                VisualEffectManager.Instance.UnSetDecayVisualEffect(brick);
            }
            if (brick.Curse != null)
            {
                brick.StopCoroutine(brick.Curse);
                brick.Curse = null;
            }
            if (brick.ExecutionCor != null)
            {
                brick.StopCoroutine(brick.ExecutionCor);
            }
            if (brick.BrickType == "ExpBrick")
            {
                ItemManager.Instance.SpawnSpecificItem("ExpBead", brick.transform.position, 10,
                    (GainValue) => brick.ShowText("+" + GainValue.ToString("F1"), "Exp", false));
            }            
            else if (brick.BrickType == "GoldBrick")
            {
                float GoldValue = 0;
                ItemManager.Instance.SpawnSpecificItem("Gold1", brick.transform.position, 3,
                    (GainValue) => GoldValue += GainValue);
                ItemManager.Instance.SpawnSpecificItem("Gold1", brick.transform.position, 4,
                    (GainValue) => GoldValue += GainValue);
                ItemManager.Instance.SpawnSpecificItem("Gold1", brick.transform.position, 3,
                    (GainValue) => GoldValue += GainValue);
                brick.ShowText("+" + GoldValue.ToString("F1"), "Gold", false);
            }
            else if (brick.BrickType != "BallBrick")
                ItemManager.Instance.SpawnItem(brick.transform.position);

            availablePositions.Add(brick.transform.position);
            usingPositions.Remove(brick.transform.position);
            Type brickType = Type.GetType(brick.BrickType);
            var method = typeof(PoolManager).GetMethod("TakeToPool");
            var genericMethod = method.MakeGenericMethod(brickType);
            genericMethod.Invoke(PoolManager, new object[] { brick.GetComponent<IBrick>() });
            CurrentBricks.Remove(brick);
        }

    }

    private void CreateSpawnBrickQueue()
    {
        int Wave = GameManager.instance.Wave;
        int AddPerWave = Wave / 10 >= 10 ? 10 : Wave / 10;
        int WaveBricks = UnityEngine.Random.Range((int)(BaseBrickSpawnValue * (10 + AddPerWave)), (int)(BaseBrickSpawnValue * (15 + AddPerWave)));
        Queue<IBrick> brickQueue = new Queue<IBrick>();
        for (int i = 0; i < WaveBricks; i++)
        {
            brickQueue.Enqueue(SettingBrick());
            if(Wave >= 5)
            {
                if (UnityEngine.Random.value < SpecialBrickSpawnChance)
                {
                    brickQueue.Enqueue(SettingSpecialBrick());
                    SpecialBrickSpawnChance = 0f;
                }
                else
                {
                    SpecialBrickSpawnChance += SpecialBrickSpawnChanceValue * (1f / (160f * BaseBrickSpawnValue));
                }
            }
        }
        if (BossSpawnCondition(Wave))
        {
            NextBossSpawnWave += BossSpawnWaveNumber;
            for (int i = 0; i < BaseBossSpawnValue; i++)
            {
                brickQueue.Enqueue(SettingBoss());
            }
        }
        SpawnBrickQueue.Enqueue(brickQueue);
    }

    public IEnumerator SpawnBricks()
    {
        while (!GameManager.instance.isGameEnded)
        {
            int Wave = GameManager.instance.Wave;
            int AddPerWave = Wave / 10 >= 10 ? 10 : Wave / 10;
            yield return new WaitUntil(() => !WavePause);

            if (BrickQueue == null)
            {
                isRapid = (UnityEngine.Random.value < RapidStageChance && GameManager.instance.Wave > 10)
                    || AugmentManager.instance.determineAugment(typeof(InfiniteRapid))
                    || (AugmentManager.instance.determineAugment(typeof(WaveFourBoss)) && BossSpawnCondition(Wave));
                SettingBrickSpawnChance();
                if(WaveRoutine != null)
                {
                    StopCoroutine(WaveRoutine);
                }
                WaveRoutine = StartCoroutine(GameManager.instance.WaveNotice());
                CreateSpawnBrickQueue();
                BrickQueue = SpawnBrickQueue.Dequeue();
            }
            /////////////////////////////////------------------------------------------------------------------------------------------
            if (isRapid) // Rapid stages are active after wave 10
            {
                RapidStageChance = 0;
                while(BrickQueue.Count > 0)
                {
                    yield return new WaitUntil(() => !WavePause);
                    if (availablePositions.Count > 0 && !WavePause)
                    {
                        SpawnBrick(BrickQueue.Dequeue());
                        yield return new WaitForSeconds(Mathf.Sin(Mathf.PI * UnityEngine.Random.value)
                            * CurrentBricks.Count < 4 ? 
                            UnityEngine.Random.Range(0.1f, 0.4f) :
                            BaseSpawnTimeValue * (0.6f - (Wave * 0.0015f) - (AddPerWave * 0.015f)) <= 0.3f ? 0.3f : (0.6f - (Wave * 0.002f) - (AddPerWave * 0.02f)));
                    }
                    else
                    {
                        SpawnBrickRoutine = null;
                        yield break;
                    }
                }
            }
            else
            {
                RapidStageChance += RapidStageChanceValue; //Ensure it activates by stage 11 at the latest
                while (BrickQueue.Count > 0)
                {
                    yield return new WaitUntil(() => !WavePause);
                    if (availablePositions.Count > 0 && !WavePause)
                    {
                        SpawnBrick(BrickQueue.Dequeue());
                        //Wait for Sin(pi * x) * 5 seconds about random value x between 0~1
                        yield return new WaitForSeconds(Mathf.Sin(Mathf.PI * UnityEngine.Random.value)
                            * CurrentBricks.Count < 4 ?
                            UnityEngine.Random.Range(0.1f, 0.4f) :
                            BaseSpawnTimeValue * (1.5f - (Wave * 0.005f) - (AddPerWave * 0.02f)) <= 0.8f ? 0.8f : (1.5f - (Wave * 0.005f) - (AddPerWave * 0.02f)));
                    }
                    else
                    {
                        SpawnBrickRoutine = null;
                        yield break;
                    }
                }
            }
            if(BrickQueue.Count <= 0)
            {
                BrickQueue = null;
                GameManager.instance.Wave += 1;
                GameManager.instance.RecordStatistics.PassedWave += 1;

                int WaitingTime = 0;
                while (WaitingTime < BaseWaitingTimeValue && CurrentBricks.Count > 5)
                {
                    WaitingTime++;
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return null;
        }
    }

    private IBrick GetBrick(IBrick brick)
    {
        float RandomNumber = UnityEngine.Random.value;
        float ChanceSum = 0;
        IBrick Brick = null;

        for (int i = 0; i < BrickInfo.BrickSpawnChance.Length; i++)
        {
            ChanceSum += BrickInfo.BrickSpawnChance[i];
            if (RandomNumber < ChanceSum)
            {
                Type brickType = Type.GetType(BrickInfo.BrickTypes[i]);
                var method = typeof(PoolManager).GetMethod("GetFromPool");
                var genericMethod = method.MakeGenericMethod(brickType);
                Brick = (IBrick)genericMethod.Invoke(PoolManager, null);
                if (Brick.BrickType == "TwinBrick" && availablePositions.Count < 2)
                {
                    Brick = null;
                    ChanceSum = 0;
                    RandomNumber = UnityEngine.Random.value;
                    i = 0;
                }
                break;
            }
        }
        Brick.Init(brick.IsBoss, brick.IsSpecialBrick, false, BrickPointSetting(brick.BrickType), BrickExpSetting(brick.BrickType));
        return Brick;
    }

    private IBrick GetSpecialBrick(IBrick brick, int index)
    {
        IBrick Brick = null;

        Type brickType = Type.GetType(BrickInfo.SpecialBrickTypes[index]);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        Brick = (IBrick)genericMethod.Invoke(PoolManager, null);
        
        Brick.Init(false, true, false, 1, 10f);
        return Brick;
    }

    private IBrick GetBossBrick(IBrick brick)
    {
        IBrick Brick = null;

        Type brickType = Type.GetType(BrickInfo.BrickTypes[CalWaveToSpawnBoss()]);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        Brick = (IBrick)genericMethod.Invoke(PoolManager, null);

        Brick.Init(true, false, false, (int)(5.3f * BrickPointSetting(brick.BrickType)), 5.3f * BrickExpSetting(brick.BrickType));
        return Brick;
    }

    private IBrick GetBigBossBrick(IBrick brick)
    {
        IBrick Brick = null;

        Type brickType = Type.GetType(BrickInfo.BrickTypes[0]);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        Brick = (IBrick)genericMethod.Invoke(PoolManager, null);

        Brick.Init(false, false, true, BrickPointSetting(brick.BrickType), BrickExpSetting(brick.BrickType));
        return Brick;
    }

    #region BrickSetting
    private int CalWaveToSpawnBoss()
    {
        int Wave = GameManager.instance.Wave;
        if (Wave < 11)
            return UnityEngine.Random.value > 0.5 ? (int)BrickType.Brick : (int)BrickType.HealBrick;
        else if (Wave < 21)
            return (int)BrickType.HardBrick;
        else if (Wave < 31)
            return (int)BrickType.SlimeBrick;
        else if (Wave < 41)
            return (int)BrickType.RebornBrick;
        else if (Wave < 51)
            return (int)BrickType.ShieldBrick;
        else if (Wave < 61)
            return (int)BrickType.TeleportBrick;
        else if (Wave < 71)
            return (int)BrickType.IgnoreBrick;
        else if (Wave < 81)
            return (int)BrickType.AbsorbBrick;
        else if (Wave < 91)
            return (int)BrickType.TwinBrick;
        else
            return UnityEngine.Random.Range(0, BrickInfo.Bricks.Length);
    }

    private int BrickPointSetting(string brickType)
    {
        if (brickType == "Brick")
            return 1;
        else if (brickType == "HardBrick")
            return 5;
        else if (brickType == "HealBrick")
            return 2;
        else if (brickType == "IgnoreBrick")
            return 5;
        else if (brickType == "ShieldBrick")
            return 3;
        else if (brickType == "SlimeBrick")
            return 2;
        else if (brickType == "TeleportBrick")
            return 3;
        else if (brickType == "TwinBrick")
            return 5;
        else if (brickType == "AbsorbBrick")
            return 5;
        else if (brickType == "RebornBrick")
            return 2;
        else
            return 1;
    }

    private float BrickExpSetting(string brickType)
    {
        if (brickType == "Brick")
            return BrickExpValue[(int)BrickType.Brick];

        else if (brickType == "HardBrick")
            return BrickExpValue[(int)BrickType.HardBrick];

        else if (brickType == "HealBrick")
            return BrickExpValue[(int)BrickType.HealBrick];

        else if (brickType == "IgnoreBrick")
            return BrickExpValue[(int)BrickType.IgnoreBrick];

        else if (brickType == "ShieldBrick")
            return BrickExpValue[(int)BrickType.ShieldBrick];

        else if (brickType == "SlimeBrick")
            return BrickExpValue[(int)BrickType.SlimeBrick];

        else if (brickType == "TeleportBrick")
            return BrickExpValue[(int)BrickType.TeleportBrick];

        else if (brickType == "TwinBrick")
            return BrickExpValue[(int)BrickType.TwinBrick];

        else if (brickType == "AbsorbBrick")
            return BrickExpValue[(int)BrickType.AbsorbBrick];

        else if (brickType == "RebornBrick")
            return BrickExpValue[(int)BrickType.RebornBrick];

        else
            return BrickExpValue[(int)BrickType.Brick];
    }

    private void SettingBrickSpawnChance()
    {
        int Wave = GameManager.instance.Wave;
        float sigma = 55f;
        float remain = 0;
        BrickInfo.BrickSpawnChance = new float[BrickInfo.BrickSpawnChance.Length];
        for (int i = 0; i < BrickInfo.BrickSpawnChance.Length - 1; i++)
        {
            float center = i * 20f;
            float x = Wave - 20;

            float value = Mathf.Exp(-(x - center) * (x - center) / (2 * sigma * sigma));
            BrickInfo.BrickSpawnChance[i] = value;
        }

        float sum = BrickInfo.BrickSpawnChance.Sum();
        if (sum > 0f)
        {
            for (int i = 0; i < BrickInfo.BrickSpawnChance.Length - 1; i++)
            {
                BrickInfo.BrickSpawnChance[i] /= sum;
                if(Wave < 11 && i == (int)BrickType.HardBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 21 && i == (int)BrickType.SlimeBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 31 && i == (int)BrickType.RebornBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 41 && i == (int)BrickType.ShieldBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 51 && i == (int)BrickType.TeleportBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 61 && i == (int)BrickType.IgnoreBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 71 && i == (int)BrickType.AbsorbBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 81 && i == (int)BrickType.TwinBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                if (BrickInfo.BrickSpawnChance[i] * 10000 < 1)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
            }
        }
        if (remain > 0f)
        {
            BrickInfo.BrickSpawnChance[(int)BrickType.Brick] += remain / 2;
            BrickInfo.BrickSpawnChance[(int)BrickType.HealBrick] += remain / 2;
        }
    }
    public IBrick SettingBrick()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = false;
        brick.IsSpecialBrick = false;
        brick.IsBigBoss = false;
        return brick;
    }

    public IBrick SettingBoss()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = true;
        brick.IsSpecialBrick = false;
        brick.IsBigBoss = false;
        return brick;
    }

    public IBrick SettingSpecialBrick()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = false;
        brick.IsSpecialBrick = true;
        brick.IsBigBoss = false;
        return brick;
    }

    public IBrick SettingBigBoss()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = false;
        brick.IsSpecialBrick = false;
        brick.IsBigBoss = true;
        return brick;
    }
    #endregion

    private IBrick GetSpecificBrick(string brickTypeName)
    {
        IBrick brick = new IBrick();

        Type brickType = Type.GetType(brickTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        brick = (IBrick)genericMethod.Invoke(PoolManager, null);
        UpdateCurrentBricks(brick);
        return brick;
    }

    private float _FusionBrickHealth;
    private int _FusionBrickNum;
    public void SpawnBrick(IBrick brick)
    {
        HPPool = new List<float>();
        int Wave = GameManager.instance.Wave;
        int AddPerWave = Wave / 10 >= 10 ? 10 : Wave / 10;
        SettingBrickHp(brick, Wave, AddPerWave);
        IBrick Brick = new IBrick();
        if (AugmentManager.instance.determineAugment(typeof(FusionBricks)) && !brick.IsBigBoss && !brick.IsSpecialBrick)
        {
            if(BrickQueue.Count == 0)
            {
                HPPool.Clear();
                HPPool.Add(_FusionBrickHealth * 0.9f);
                brick.IsBoss = true;
                brick.IsSpecialBrick = false;
                Brick = GetBrick(brick);

                Brick.Point *= _FusionBrickNum;
                Brick.Exp *= _FusionBrickNum;

                Vector2 randomPosition = GenerateRandomPosition();
                Brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
                if(Brick.IsBoss)
                    SoundManager.instance.SpawnBossSound(Brick.GetComponent<AudioSource>());

                else
                    SoundManager.instance.SpawnBrickSound(Brick.GetComponent<AudioSource>());
                _FusionBrickHealth = 0;
                _FusionBrickNum = 0;
            }
        }
        else
        {
            if (brick.IsBoss)
            {
                Brick = GetBossBrick(brick);
            }
            else if (brick.IsSpecialBrick)
                Brick = GetSpecialBrick(brick, UnityEngine.Random.Range(0, BrickInfo.SpecialBrickTypes.Length));
            else if (brick.IsBigBoss)
            {
                Brick = GetBigBossBrick(brick);
                BigBossBrick = Brick;
            }
            else
                Brick = GetBrick(brick);

            Vector2 randomPosition = GenerateRandomPosition();
            Brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
            SoundManager.instance.SpawnBrickSound(Brick.GetComponent<AudioSource>());
        }
        if(Brick.BrickType == "TwinBrick" && Brick.GetComponent<TwinBrick>().Twin == null)
        {
            IBrick Twin = SpawnSpecificBrick("TwinBrick");
            Brick.GetComponent<TwinBrick>().Twin = Twin.GetComponent<TwinBrick>();
            Twin.GetComponent<TwinBrick>().Twin = Brick.GetComponent <TwinBrick>();
            Brick.GetComponent<Collider2D>().enabled = true;
            Twin.GetComponent<Collider2D>().enabled = true;
            float rotation = UnityEngine.Random.value * 3.14f;
            Brick.spriteRenderer.material.SetFloat("_Rotation", rotation);
            Twin.spriteRenderer.material.SetFloat("_Rotation", rotation);
            Twin.Init(Brick.IsBoss, false, false, Brick.Point, Brick.Exp);
        }
        
        if (Brick.IsBoss)
        {
            WavePause = true;
            if (AugmentManager.instance.determineAugment(typeof(DoubleBoss)))
            {
                int BossSpawned = 1;
                for(int i = 0; i < CurrentBricks.Count; i++)
                {
                    if (CurrentBricks[i].IsBoss)
                        BossSpawned += 1;
                }
                if (BossSpawned == 2)
                    WavePause = true;
                else
                    WavePause = false;
            }
        }

        if (AugmentManager.instance.determineAugment(typeof(PenetratingBall)))
        {
            Brick.GetComponent<Collider2D>().isTrigger = true;
        }
        else
            Brick.GetComponent<Collider2D>().isTrigger = false;
        UpdateCurrentBricks(Brick);
    }

    public IBrick SpawnSpecificBrick(string BrickType)
    {
        IBrick brick = null;
        if (availablePositions.Count > 0)
        {
            Vector2 randomPosition = GenerateRandomPosition();
            brick = GetSpecificBrick(BrickType);
            brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
            UpdateCurrentBricks(brick);
        }
        SoundManager.instance.SpawnBrickSound(brick.GetComponent<AudioSource>());
        return brick;
    }

    private void SettingBrickHp(IBrick brick, int Wave, int AddPerWave)
    {
        if (brick.IsBoss)
        {
            for (int i = 0; i < 3; i++)
            {
                HPPool.Add((int)(BaseBossHealthValue * (Mathf.Pow((Wave >= 3 ? (Wave / 1f) : 2) + (AddPerWave * 5) + 1, 1.2f) * UnityEngine.Random.Range(4.9f, 5.1f))));
            }
            _FusionBrickHealth += HPPool.Average();
        }
        else if (brick.IsSpecialBrick)
        {
            for (int i = 0; i < 3; i++)
            {
                HPPool.Add(UnityEngine.Random.Range(5, 10));
            }
        }
        else if (brick.IsBigBoss)
        {
            HPPool.Add(2); //10101);
        }
        else
        {
            //need to adjust
            for (int j = 0; j < 3 * 1.1; j++)
            {
                HPPool.Add((int)(BaseBrickHealthValue * (Mathf.Pow((Wave >= 3 ? (Wave / 1f) : 2) + (AddPerWave * 5) + 1, 1.1f)) * UnityEngine.Random.Range(0.9f, 1.1f)));
            }
            _FusionBrickHealth += HPPool.Average();
        }
        _FusionBrickNum++;
    }

    public bool BossSpawnCondition(int Wave)
    {
        bool isBossSpawnCondition = Wave == NextBossSpawnWave;

        return isBossSpawnCondition; 
    }

    public void WaveSkip(int Num)
    {
        if (BrickQueue != null)
        {
            BrickQueue = null;
        }
        if (SpawnBrickRoutine != null)
        {
            StopCoroutine(SpawnBrickRoutine);
            SpawnBrickRoutine = null;
        }
        GameManager.instance.Wave += Num;
        if(Num > 0)
        {
            GameManager.instance.RecordStatistics.PassedWave += Num;
        }
        SpawnBrickRoutine = StartCoroutine(SpawnBricks());
    }

    public void DestroyBricksNum(int Num, bool isBrickDied = false)
    {
        var bricksCopy = CurrentBricks.ToList();
        if (isBrickDied)
        {
            for (int i = 0; i < Mathf.Min(Num, bricksCopy.Count); i++)
            {
                if (!CurrentBricks[i].IsBigBoss)
                    CurrentBricks[i].BrickDie();
            }
        }
        else
        {
            for (int i = 0; i < Mathf.Min(Num, bricksCopy.Count); i++)
            {
                if (!bricksCopy[i].IsBigBoss)
                    TakeBrick(bricksCopy[i]);
            }
        }
    }

    public IBrick GetRandomBrick()
    {
        bool CheckIsThereBrick = false;
        for(int i = 0; i < CurrentBricks.Count; i++)
        {
            if (!CurrentBricks[i].IsSpecialBrick)
                CheckIsThereBrick = true;
        }

        if (!CheckIsThereBrick) return null;
        IBrick brick = CurrentBricks[UnityEngine.Random.Range(0, CurrentBricks.Count)];

        if(CheckIsThereBrick)
            while (!brick.IsSpecialBrick)
            {
                brick = CurrentBricks[UnityEngine.Random.Range(0, CurrentBricks.Count)];
            }
        return brick;
    }

    //Test Button Functions
    public void OnSpawnWaveBrickButtonClick()
    {
        try
        {
            if(availablePositions.Count > 0)
            {
                SpawnBrick(BrickQueue.Dequeue());
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log("BrickQueue is empty: " + e.Message);
        }
    }

    public void OnBrickSpawnButtonClick()
    {
        if(availablePositions.Count > 0)
        {
            SpawnBrick(SettingBrick());
        }
    }

    public void OnSpecialBrickSpawnButtonClick()
    {
        if(availablePositions.Count > 0)
        {
            SpawnBrick(SettingSpecialBrick());
        }
    }

    public void OnBossSpawnButtonClick()
    {
        if(availablePositions.Count > 0)
        {
            SpawnBrick(SettingBoss());
        }
    }

    public void OnWaveStopButtonClick()
    {
        WavePause = !WavePause;
        Debug.Log("Now WaveStop : " + WavePause);
    }

    public void OnSkipWaveButtonClick()
    {
        WaveSkip(1);
    }
    //*******************************************************
}
