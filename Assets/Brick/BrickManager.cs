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

    enum BrickType
    {
        Brick,
        HealBrick,
        ShieldBrick,
        SlimeBrick,
        HardBrick,
        TeleportBrick,
        AbsorbBrick,
        IgnoreBrick,
        TwinBrick,
    }

    enum SpecialBrickType
    {
        ExpBrick,
    }

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
        SpawnBrickRoutine = StartCoroutine(SpawnBricks());
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

    void Update()
    {
        if(availablePositions.Count > 0 && SpawnBrickRoutine == null && !WavePause && !GameManager.instance.isGameEnded)
        {
            SpawnBrickRoutine = StartCoroutine(SpawnBricks());
        }
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
            OnWaveStopButtonClick();
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

    private IEnumerator SpawnBricks()
    {
        while (!GameManager.instance.isGameEnded)
        {
            int Wave = GameManager.instance.Wave;
            int AddPerWave = Wave / 10 >= 10 ? 10 : Wave / 10;
            if (BrickQueue == null)
            {
                yield return new WaitUntil(() => !WavePause);
                isRapid = (UnityEngine.Random.value < RapidStageChance && GameManager.instance.Wave > 10)
                    || AugmentManager.instance.determineAugmentID("SPECIAL_62")
                    || (AugmentManager.instance.determineAugmentID("SPECIAL_72") && BossSpawnCondition(Wave));
                SettingBrickSpawnChance();
                StartCoroutine(GameManager.instance.WaveNotice());
                CreateSpawnBrickQueue();
                BrickQueue = SpawnBrickQueue.Dequeue();
            }
            /////////////////////////////////------------------------------------------------------------------------------------------
            if (isRapid) // Rapid stages are active after wave 10
            {
                RapidStageChance = 0;
                while(BrickQueue.Count > 0)
                {
                    if(availablePositions.Count > 0 && !WavePause)
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
                    if(availablePositions.Count > 0 && !WavePause)
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
        Brick.Init(brick.IsBoss, brick.IsSpecialBrick, BrickPointSetting(brick.BrickType), BrickExpSetting(brick.BrickType));
        return Brick;
    }

    private IBrick GetSpecialBrick(IBrick brick, int index)
    {
        IBrick Brick = null;

        Type brickType = Type.GetType(BrickInfo.SpecialBrickTypes[index]);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        Brick = (IBrick)genericMethod.Invoke(PoolManager, null);
        
        Brick.Init(false, true, 1, 10f);
        return Brick;
    }

    private IBrick GetBossBrick(IBrick brick)
    {
        IBrick Brick = null;

        Type brickType = Type.GetType(BrickInfo.BrickTypes[CalWaveToSpawnBoss()]);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        Brick = (IBrick)genericMethod.Invoke(PoolManager, null);

        Brick.Init(true, false, (int)(5.3f * BrickPointSetting(brick.BrickType)), 5.3f * BrickExpSetting(brick.BrickType));
        return Brick;
    }

    private int CalWaveToSpawnBoss()
    {
        int Wave = GameManager.instance.Wave;
        if (Wave < 11)
            return (int)BrickType.Brick;
        else if (Wave < 21)
            return (int)BrickType.HealBrick;
        else if (Wave < 31)
            return (int)BrickType.ShieldBrick;
        else if (Wave < 41)
            return (int)BrickType.SlimeBrick;
        else if (Wave < 51)
            return (int)BrickType.HardBrick;
        else if (Wave < 61)
            return (int)BrickType.TeleportBrick;
        else if (Wave < 71)
            return (int)BrickType.TwinBrick;
        else if (Wave < 81)
            return (int)BrickType.AbsorbBrick;
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
        else
            return 1;
    }

    private float BrickExpSetting(string brickType)
    {
        if (brickType == "Brick")
            return 10f;
        else if (brickType == "HardBrick")
            return 20f;
        else if (brickType == "HealBrick")
            return 12.5f;
        else if (brickType == "IgnoreBrick")
            return 20f;
        else if (brickType == "ShieldBrick")
            return 15.5f;
        else if (brickType == "SlimeBrick")
            return 12.5f;
        else if (brickType == "TeleportBrick")
            return 15.5f;
        else if (brickType == "TwinBrick")
            return 20f;
        else if (brickType == "AbsorbBrick")
            return 10f;
        else
            return 10f;
    }
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

    private void SettingBrickSpawnChance()
    {
        int Wave = GameManager.instance.Wave;
        float sigma = 20f;
        float remain = 0;
        BrickInfo.BrickSpawnChance = new float[BrickInfo.BrickSpawnChance.Length];
        for (int i = 0; i < BrickInfo.BrickSpawnChance.Length - 1; i++)
        {
            float center = i * 20f;
            float x = Wave < 80 ? Wave : 80;

            float value = Mathf.Exp(-(x - center) * (x - center) / (2 * sigma * sigma));
            BrickInfo.BrickSpawnChance[i] = value;
        }
        
        float sum = BrickInfo.BrickSpawnChance.Sum();
        if (sum > 0f)
        {
            for (int i = 0; i < BrickInfo.BrickSpawnChance.Length - 1; i++)
            {
                BrickInfo.BrickSpawnChance[i] /= sum;
                if(Wave < 11 && i == (int)BrickType.HealBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 21 && i == (int)BrickType.ShieldBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 31 && i == (int)BrickType.SlimeBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 41 && i == (int)BrickType.HardBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 51 && i == (int)BrickType.TeleportBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 61 && i == (int)BrickType.AbsorbBrick)
                {
                    remain += BrickInfo.BrickSpawnChance[i];
                    BrickInfo.BrickSpawnChance[i] = 0;
                }
                else if (Wave < 71 && i == (int)BrickType.IgnoreBrick)
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
        if(remain > 0f)
        {
            BrickInfo.BrickSpawnChance[0] += remain;
        }
    }
    public IBrick SettingBrick()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = false;
        brick.IsSpecialBrick = false;
        return brick;
    }

    public IBrick SettingBoss()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = true;
        brick.IsSpecialBrick = false;
        return brick;
    }

    public IBrick SettingSpecialBrick()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = false;
        brick.IsSpecialBrick = true;
        return brick;
    }

    private float _FusionBrickHealth;
    private int _FusionBrickNum;
    public void SpawnBrick(IBrick brick)
    {
        HPPool = new List<float>();
        int Wave = GameManager.instance.Wave;
        int AddPerWave = Wave / 10 >= 10 ? 10 : Wave / 10;
        if (brick.IsBoss)
        {
            OnWaveStopButtonClick();
            for (int i = 0; i < 3; i++)
            {
                HPPool.Add((int)(BaseBossHealthValue * (Mathf.Pow((Wave >= 3 ? (Wave / 1.3f) : 2) + (AddPerWave * 2) + 1, 1.1f) * UnityEngine.Random.Range(4f, 5f))));
            }
        }
        else if (brick.IsSpecialBrick)
        {
            for (int i = 0; i < 3; i++)
            {
                HPPool.Add(UnityEngine.Random.Range(5, 10));
            }
        }
        else
        {
            int HPBase = (int)(BaseBrickHealthValue * (Mathf.Pow((Wave >= 3 ? (Wave / 1.3f) : 2) + (AddPerWave * 2) + 1, UnityEngine.Random.Range(0.9f, 1.1f))));
            //need to adjust
            for (int j = HPBase; j < 3 + HPBase * 1.1; j++)
            {
                HPPool.Add(j);
            }
        }
        _FusionBrickHealth += HPPool.Average();
        _FusionBrickNum++;
        IBrick Brick = new IBrick();
        if (AugmentManager.instance.determineAugmentID("SPECIAL_65"))
        {
            if(BrickQueue.Count == 0)
            {
                HPPool.Clear();
                HPPool.Add(_FusionBrickHealth);
                Debug.Log(_FusionBrickHealth);
                brick.IsBoss = true;
                brick.IsSpecialBrick = false;
                Brick = GetBrick(brick);

                Brick.Point *= _FusionBrickNum;
                Brick.Exp *= _FusionBrickNum;

                Vector2 randomPosition = GenerateRandomPosition();
                Brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
                SoundManager.instance.SpawnBrickSound(Brick.GetComponent<AudioSource>());
                _FusionBrickHealth = 0;
                _FusionBrickNum = 0;
            }
        }
        else
        {
            if (brick.IsBoss)
                Brick = GetBossBrick(brick);
            else if (brick.IsSpecialBrick)
                Brick = GetSpecialBrick(brick, UnityEngine.Random.Range(0, BrickInfo.SpecialBrickTypes.Length));
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

            float rotation = UnityEngine.Random.value * 3.14f;
            Brick.spriteRenderer.material.SetFloat("_Rotation", rotation);
            Twin.spriteRenderer.material.SetFloat("_Rotation", rotation);
            Twin.Init(Brick.IsBoss, false, Brick.Point, Brick.Exp);
        }
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

    public bool BossSpawnCondition(int Wave)
    {
        bool isBossSpawnCondition = Wave == NextBossSpawnWave;

        return isBossSpawnCondition; 
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
        if(BrickQueue.Count > 0)
        {
            BrickQueue.Clear();
        }
        if(BrickQueue != null)
        {
            BrickQueue = null;
        }
        if(SpawnBrickRoutine != null)
        {
            StopCoroutine(SpawnBrickRoutine);
            SpawnBrickRoutine = null;
        }
        GameManager.instance.Wave += 1;
    }
    //*******************************************************
}
