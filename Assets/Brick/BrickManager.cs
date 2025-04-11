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

    void Awake()
    {
        Instance = this;
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        InitializeGrid();
        PoolManager = GetComponent<PoolManager>();
    }

    private void Start()
    {
        //StartCoroutine(SpawnBricks());
    }

    void Update()
    {
        
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

    private IBrick GetBrick()
    {
        float RandomNumber = UnityEngine.Random.value;
        IBrick brick = null;

        float ChanceSum = 0;

        for(int i = 0; i < BrickInfo.BrickSpawnChance.Length; i++)
        {
            ChanceSum += BrickInfo.BrickSpawnChance[i];
            if (RandomNumber < ChanceSum)
            {
                Type brickType = Type.GetType(BrickInfo.BrickTypes[i]);
                var method = typeof(PoolManager).GetMethod("GetFromPool");
                var genericMethod = method.MakeGenericMethod(brickType);
                brick = (IBrick)genericMethod.Invoke(PoolManager, null);
                break;
            }
        }
        //UpdateCurrentBricks();
        return brick;
    }
    
    private IBrick GetSpecificBrick(string brickTypeName)
    {
        IBrick brick = null;
        
        Type brickType = Type.GetType(brickTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        brick = (IBrick)genericMethod.Invoke(PoolManager, null);
        //UpdateCurrentBricks();
        return brick;
    }

    private IEnumerator SpawnBricks()
    {
        while (true)
        {
            int Wave = UnityEngine.Random.Range(10, 20);
            for (int i = 0; i < Wave; i++)
            {
                SpawnBrick();
                //Wait for Sin(pi * x) * 5 seconds about random value x between 0~1
                yield return new WaitForSeconds(Mathf.Sin(Mathf.PI * UnityEngine.Random.value) * 5);
            }
            GameTick += 1;
            bool IsBossSpawn = (UnityEngine.Random.value < 0.5) ? true : false;
            if(IsBossSpawn) //Boss Spawn
            {
                SpawnBoss();
            }
            else // Start Rapid Stage
            {
                for (int i = 0; i < Wave; i++)
                {
                    SpawnBrick();
                    yield return new WaitForSeconds(Mathf.Sin(Mathf.PI * UnityEngine.Random.value) * 0.5f);
                }
            }
        }
    }

    public void SpawnBrick()
    {
        HPPool = new List<float>();
        int HPBase = (int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, UnityEngine.Random.Range(0.9f, 1.1f));
        //need to adjust
        for (int j = HPBase; j < 3 + HPBase * 1.1; j++)
        {
            HPPool.Add(j);
        }
        if (availablePositions.Count > 0)
        {
            Vector2 randomPosition = GenerateRandomPosition();
            IBrick brick = GetBrick();
            brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
            UpdateCurrentBricks(brick);
        }
    }

    public void SpawnBoss()
    {
        HPPool = new List<float>();
        for(int i = 0; i < 3; i++)
        {
            HPPool.Add((int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, 1.1f) * UnityEngine.Random.Range(8, 12));
        }
        Vector2 randomPosition = GenerateRandomPosition();
        IBrick brick = GetBrick();
        brick.IsBoss = true;
        brick.transform.position = new Vector3(randomPosition.x, randomPosition.y, 35);
        UpdateCurrentBricks(brick);
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
        return brick;
    }
}
