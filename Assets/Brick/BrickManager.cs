using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
public class BrickManager : MonoBehaviour
{
    public List<float> HPPool;
    private int GameTick = 1;

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

    void Awake()
    {
        Instance = this;
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        InitializeGrid();
        SpawnBrickQueue = new Queue<Queue<IBrick>>();
        BrickQueue = new Queue<IBrick>();
        PoolManager = GetComponent<PoolManager>();
        SpawnBrickRoutine = null;
    }

    private void Start()
    {
        StartCoroutine(SpawnBricks());
    }

    void Update()
    {
        if(availablePositions.Count > 0 && SpawnBrickRoutine == null)
        {
            SpawnBrickRoutine = StartCoroutine(SpawnBricks());
        }
    }

    public void UpdateCurrentBricks(IBrick brick)
    {
        if(!CurrentBricks.Contains(brick))
        {
            if(brick.BrickType == "TwinBrick")
            {
                CurrentBricks.Add(brick.GetComponent<TwinBrick>());
                CurrentBricks.Add(brick.GetComponentInChildren<LowerBrick>());
            }
            else
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
        if(brick.gameObject.activeSelf)
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
            if(brick.BrickType == "TwinBrick") CurrentBricks.Remove(brick.GetComponentInChildren<LowerBrick>());
        }

    }

    Queue<Queue<IBrick>> SpawnBrickQueue;
    public Queue<IBrick> BrickQueue;

    private bool isMiniBossSpanwed = false;
    private float MiniBossSpawnChance = 0;
    public float MiniBossSpawnChanceValue = 0.15f;

    private float RapidStageChance = 0;
    public float RapidStageChanceValue = 0.09f;

    public float BaseBrickSpawnValue = 10;

    private void CreateSpawnBrickQueue()
    {
        int WaveBricks = UnityEngine.Random.Range((int)BaseBrickSpawnValue + GameTick, (int)BaseBrickSpawnValue + 5 + GameTick);
        Queue<IBrick> brickQueue = new Queue<IBrick>();
        if (GameManager.instance.Wave % 10 != 0)
        {
            if (!isMiniBossSpanwed && GameManager.instance.Wave >= 5) //After wave 5, Mini boss can spawn
            {
                if (MiniBossSpawnChance < UnityEngine.Random.value)
                {
                    brickQueue.Enqueue(SettingMiniBoss());
                    MiniBossSpawnChance = 0f;
                    isMiniBossSpanwed = true;
                }
                else
                {
                    MiniBossSpawnChance += MiniBossSpawnChanceValue; // Ensure it activates by stage 7 at the latest
                }
            }
        }
        else
        {
            brickQueue.Enqueue(SettingBoss());
            isMiniBossSpanwed = false;
        }
        for (int i = 0; i < WaveBricks; i++)
        {
            brickQueue.Enqueue(SettingBoss());
            brickQueue.Enqueue(SettingBrick());
        }
        SpawnBrickQueue.Enqueue(brickQueue);
    }

    private IEnumerator SpawnBricks()
    {
        while (true)
        {
            if (BrickQueue == null)
            {
                CreateSpawnBrickQueue();
                BrickQueue = SpawnBrickQueue.Dequeue();
                Debug.Log("Wave : " + GameManager.instance.Wave + " Left Bricks Count : " + BrickQueue.Count);
            }
            else
            {
                Debug.Log("Wave : " + GameManager.instance.Wave + " Left Bricks Count : " + BrickQueue.Count);
            }
            /////////////////////////////////------------------------------------------------------------------------------------------
            if (RapidStageChance < UnityEngine.Random.value && GameManager.instance.Wave >= 5) // Rapid stages are active after wave 5
            {
                Debug.Log("Rapid Stage");
                RapidStageChance = 0;
                while(BrickQueue.Count > 0)
                {
                    if(availablePositions.Count > 0)
                    {
                        SpawnBrick(BrickQueue.Dequeue());
                        yield return new WaitForSeconds(Mathf.Sin(Mathf.PI * UnityEngine.Random.value) * 0.6f);
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
                Debug.Log("Normal Stage");
                RapidStageChance += RapidStageChanceValue; //Ensure it activates by stage 11 at the latest
                while (BrickQueue.Count > 0)
                {
                    if(availablePositions.Count > 0)
                    {
                        SpawnBrick(BrickQueue.Dequeue());
                        //Wait for Sin(pi * x) * 5 seconds about random value x between 0~1
                        yield return new WaitForSeconds(Mathf.Sin(Mathf.PI * UnityEngine.Random.value) * 3);
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
                GameTick += 1;
                GameManager.instance.Wave += 1;
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
                break;
            }
        }
        Brick.Init(brick.IsBoss, brick.IsMiniBoss);
        return Brick;
    }

    private IBrick GetSpecificBrick(string brickTypeName)
    {
        IBrick brick = new IBrick();

        Type brickType = Type.GetType(brickTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        brick = (IBrick)genericMethod.Invoke(PoolManager, null);
        //UpdateCurrentBricks();
        return brick;
    }

    public IBrick SettingBrick()
    {
        IBrick brick = new IBrick();
        return brick;
    }

    public IBrick SettingBoss()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = true;
        return brick;
    }

    public IBrick SettingMiniBoss()
    {
        IBrick brick = new IBrick();
        brick.IsBoss = true;
        return brick;
    }

    public void SpawnBrick(IBrick brick)
    {
        HPPool = new List<float>();
        if (brick.IsBoss)
        {
            for (int i = 0; i < 3; i++)
            {
                HPPool.Add((int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, 1.1f) * UnityEngine.Random.Range(11, 14));
            }
        }
        else if (brick.IsMiniBoss)
        {
            for (int i = 0; i < 3; i++)
            {
                HPPool.Add((int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, 1.1f) * UnityEngine.Random.Range(6, 8));
            }
        }
        else
        {
            int HPBase = (int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, UnityEngine.Random.Range(0.9f, 1.1f));
            //need to adjust
            for (int j = HPBase; j < 3 + HPBase * 1.1; j++)
            {
                HPPool.Add(j);
            }
        }
        IBrick Brick = GetBrick(brick);

        Vector2 randomPosition = GenerateRandomPosition();
        Brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
        UpdateCurrentBricks(Brick);
        SoundManager.instance.SpawnBrickSound(Brick.GetComponent<AudioSource>());
        if (Brick.IsBoss)
        {
            Debug.Log("Boss Awake");
        }
        else if(Brick.IsMiniBoss)
        {
            Debug.Log("MiniBoss Awake");
        }
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
}
