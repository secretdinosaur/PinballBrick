using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public SpriteRenderer[] Bricks;
    void Start()
    {
        InitializeGrid();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Vector2> availablePositions = new List<Vector2>();
    public List<Vector2> usingPositions = new List<Vector2>();
    void InitializeGrid()
    {
        Bricks = GetComponentsInChildren<SpriteRenderer>();

        float cellWidth = 4.75f / 6;
        float cellHeight = 6f / 10;
        availablePositions = new List<Vector2>();
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                float posX = -2.375f + x * cellWidth + cellWidth / 2;
                float posY = -1.75f + y * cellHeight + cellHeight / 2;
                availablePositions.Add(new Vector2(posX, posY));
            }
        }
        for(int i = 0; i < 60; i++)
        {
            Bricks[i].transform.position = availablePositions[i];
        }
    }

    private void BackgroundProcess()
    {
    }
}
