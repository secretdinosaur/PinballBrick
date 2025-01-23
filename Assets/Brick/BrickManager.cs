using UnityEngine;
using Redcode.Pools;
using System.Collections.Generic;
public class BrickManager : MonoBehaviour
{
    public int BrickStage = 1;
    public static BrickManager Instance;
    public IBrick[] CurrentBricks;
    public GameObject Brick;
    private PoolManager PoolManager;

    private float ScreenWidth;
    private float ScreenHeight;

    private float HorizontalGridSize = 6;
    private float VerticalGridSize = 16;

    private int minYGrid = 5;

    private List<Vector2> availablePositions = new List<Vector2>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        InitializeGrid();
        PoolManager = GetComponent<PoolManager>();
        UpdateCurrentBricks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentBricks()
    {
        CurrentBricks = GetComponentsInChildren<IBrick>();
        if(CurrentBricks.Length == 0)
        {
            CreateBricks();
        }
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

    List<Vector2> GenerateRandomPositions(int count)
    {
        List<Vector2> selectedPositions = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availablePositions.Count);
            Vector2 selectedPosition = availablePositions[randomIndex];

            selectedPositions.Add(selectedPosition);
            availablePositions.RemoveAt(randomIndex);
        }

        return selectedPositions;
    }
    public void CreateBricks()
    {
        InitializeGrid();
        List<Vector2> randomPositions = GenerateRandomPositions(10);

        foreach (var pos in randomPositions)
        {
            IBrick brick = GetBrick();
            brick.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 35));
        }
        BrickStage++;
    }

    public void TakeBrick(IBrick brick)
    {
        if(brick.gameObject.activeSelf)
        {
            ItemManager.Instance.SpawnItem(brick.transform.position);

            switch(brick.Type)
            {
                case "Hard":
                    PoolManager.TakeToPool<HardBrick>(brick);
                    break;
                case "Shield":
                    PoolManager.TakeToPool<ShieldBrick>(brick);
                    break;
                case "Basic":
                    PoolManager.TakeToPool<Brick>(brick);
                    break;
            }
            UpdateCurrentBricks();
        }
    }

    private IBrick GetBrick()
    {
        int RandomNumber = Random.Range(0, 100);
        IBrick brick;
        if(RandomNumber < 10)
        {
            brick = PoolManager.GetFromPool<HardBrick>();
            brick.Type = "Hard";
        }
        else if (RandomNumber < 70)
        {
            brick = PoolManager.GetFromPool<ShieldBrick>();
            brick.Type = "Shield";
        }
        else
        {
            brick = PoolManager.GetFromPool<IBrick>();
            brick.Type = "Basic";
        }
        return brick;
    }
}
