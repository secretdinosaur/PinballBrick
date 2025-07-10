using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Redcode.Pools;
public class Background : MonoBehaviour
{
    public SpriteRenderer[] Bricks;
    public static Background Instance;
    public PoolManager PoolManager;

    public GameObject ScrollContent;

    private Coroutine Routine1;
    private Coroutine Routine2;
    void Start()
    {
        Instance = this;
        PoolManager = GetComponent<PoolManager>();
    }

    private void Awake()
    {
        InitializeGrid();
        Routine1 = StartCoroutine(BackgroundProcess());
        Routine2 = StartCoroutine(BackgroundProcess2());
    }

    // Update is called once per frame
    void Update()
    {
        if(ScrollContent.GetComponent<RectTransform>().anchoredPosition.x != - 3600)
        {
            if(Routine1 != null)
                StopCoroutine(Routine1);
            if(Routine2 != null)
                StopCoroutine(Routine2);
            Routine1 = null;
            Routine2 = null;
        }
        else
        {
            if (Routine1 == null)
                Routine1 = StartCoroutine(BackgroundProcess());
            if (Routine2 == null)
                Routine2 = StartCoroutine(BackgroundProcess2());

        }

    }
    public List<Vector2> availablePositions = new List<Vector2>();
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
                availablePositions.Add(new Vector3(posX, posY, 130));
            }
        }
        for(int i = 0; i < 60; i++)
        {
            Bricks[i].transform.localPosition = availablePositions[i];
            Bricks[i].gameObject.SetActive(false);
        }
    }

    IEnumerator BackgroundProcess()
    {
        while(true)
        {
            Bricks[Random.Range(0, 60)].gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        }
    }

    IEnumerator BackgroundProcess2()
    {
        while (true)
        {
            for(int i = Random.Range(0,5); i < 5; i++)
            {
                BallForMainScene Ball = PoolManager.GetFromPool<BallForMainScene>();
                Ball.transform.position = new Vector3(Random.Range(-2f, 2f), 5.25f);
                Ball.GetComponent<Rigidbody2D>().linearVelocityX = Random.Range(-1f, 1f);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(Random.Range(1, 3f));
        }
    }

    public void TakeBall(BallForMainScene Ball)
    {
        PoolManager.TakeToPool<BallForMainScene>(Ball);
    }
}
