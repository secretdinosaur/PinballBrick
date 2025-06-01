using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System;

public class GhostBall : IBall
{
    void OnEnable()
    {
        base.OnEnable();
        BallType = "GhostBall";
        BallBaseDamage = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StartGhostBall());
    }

    public void Init(Sprite BallSprite, float BallBaseDamage)
    {
        spriteRenderer.sprite = BallSprite;
        this.BallBaseDamage = BallBaseDamage;
    }

    IEnumerator StartGhostBall()
    {
        int time = 0;
        bool isFallen = false;
        DOVirtual.Int(0, 10, 10, value =>
        {
            time = value;
        }).OnComplete(() => isFallen = true);

        yield return new WaitUntil(() => isFallen || transform.position.y <= -10);
        spriteRenderer.DOFade(0f, 1f)
            .OnComplete(() => GhostBallDisappearProcess());
    }

    void GhostBallDisappearProcess()
    {
        for (int i = 0; i < AugmentManager.instance.CurseActiveNumber; i++)
        {
            int index = Random.Range(0, BrickManager.Instance.CurrentBricks.Count);
            if (BrickManager.Instance.CurrentBricks.Count != 0)
                while (BrickManager.Instance.CurrentBricks[index].RecentlyBall == null)
                {
                    index = Random.Range(0, BrickManager.Instance.CurrentBricks.Count);
                }
            try
            {
                BrickManager.Instance.CurrentBricks[index].DamageByCurse();
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.Log("There are no Bricks");
            }
        }
        BallManager.Instance.TakeBall(this);
    }
}
