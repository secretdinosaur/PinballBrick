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
    public IBrick[] CurrentBricks;
    private PoolManager PoolManager;

    private float ScreenWidth;
    private float ScreenHeight;

    private float HorizontalGridSize = 6;
    private float VerticalGridSize = 16;

    private int minYGrid = 5;

    public BrickInfo BrickInfo;

    public List<Vector2> availablePositions = new List<Vector2>();
    public List<Vector2> usingPositions = new List<Vector2>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        InitializeGrid();
        PoolManager = GetComponent<PoolManager>();
        UpdateCurrentBricks();
        StartCoroutine(SpawnBricks());
    }
    
    void Update()
    {
        
    }

    public void UpdateCurrentBricks()
    {
        CurrentBricks = GetComponentsInChildren<IBrick>();
    }
    void InitializeGrid()
    {
        float cellWidth = ScreenWidth / HorizontalGridSize;
        float cellHeight = ScreenHeight / VerticalGridSize;
        availablePositions = new List<Vector2>();
        for (int x = 0; x < HorizontalGridSize; x++)
        {
            for (int y = minYGrid; y < VerticalGridSize - 1; y++)
            {
                float posX = x * cellWidth + cellWidth / 2;
                float posY = y * cellHeight + cellHeight / 2;
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

    public void TeleportBrickPosition(TeleportBrick brick)
    {
        availablePositions.Add(Camera.main.WorldToScreenPoint(brick.transform.position));
        usingPositions.Remove(Camera.main.WorldToScreenPoint(brick.transform.position));
        Vector2 RandomPosition = GenerateRandomPosition();
        brick.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(RandomPosition.x, RandomPosition.y, 35));
    }
    public void TakeBrick(IBrick brick)
    {
        if(brick.gameObject.activeSelf)
        {
            ItemManager.Instance.SpawnItem(brick.transform.position);

            availablePositions.Add(Camera.main.WorldToScreenPoint(brick.transform.position));
            usingPositions.Remove(Camera.main.WorldToScreenPoint(brick.transform.position));

            Type brickType = Type.GetType(brick.BrickType);
            var method = typeof(PoolManager).GetMethod("TakeToPool");
            var genericMethod = method.MakeGenericMethod(brickType);
            genericMethod.Invoke(PoolManager, new object[] { brick.GetComponent<IBrick>() });

            UpdateCurrentBricks();
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
        UpdateCurrentBricks();
        return brick;
    }
    
    private IBrick GetSpecificBrick(string brickTypeName)
    {
        IBrick brick = null;
        
        Type brickType = Type.GetType(brickTypeName);
        var method = typeof(PoolManager).GetMethod("GetFromPool");
        var genericMethod = method.MakeGenericMethod(brickType);
        brick = (IBrick)genericMethod.Invoke(PoolManager, null);
        UpdateCurrentBricks();
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
        int HPBase = (int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, 1.1f);
        //need to adjust
        for (int j = HPBase; j < 3 + HPBase; j++)
        {
            HPPool.Add(j);
        }
        if (availablePositions.Count > 0)
        {
            Vector2 randomPosition = GenerateRandomPosition();
            IBrick brick = GetBrick();
            brick.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(randomPosition.x, randomPosition.y, 35));
        }
    }

    public void SpawnBoss()
    {
        HPPool = new List<float>();
        HPPool.Add((int)Mathf.Pow(LevelManager.Instance.Level + GameTick + 10, 1.1f) * 10);
        Vector2 randomPosition = GenerateRandomPosition();
        IBrick brick = GetBrick();
        brick.IsBoss = true;
        brick.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(randomPosition.x, randomPosition.y, 35));
    }

    public IBrick SpawnSpecificBrick(string BrickType)
    {
        IBrick brick = null;
        if (availablePositions.Count > 0)
        {
            Vector2 randomPosition = GenerateRandomPosition();
            brick = GetSpecificBrick(BrickType);
            brick.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(randomPosition.x, randomPosition.y, 35));
        }

        return brick;
    }
}
