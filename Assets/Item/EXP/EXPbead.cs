using UnityEngine;
using DG.Tweening;

public class ExpBead : Item
{
    public float ExpAmount;
    public override void MoveItem()
    {
        Vector3 screenMidRight = new Vector3(Screen.width / 2, Screen.height, Camera.main.nearClipPlane);
        Vector3 worldTopRight = Camera.main.ScreenToWorldPoint(screenMidRight);

        float angle = Random.Range(135f, 315f);
        Vector3 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(transform.position + direction * Random.Range(0.08f, 0.12f), Random.Range(0.3f, 0.7f)).SetEase(Ease.OutCirc));
        seq.Append(transform.DOMove(worldTopRight, 0.5f).SetEase(Ease.InCirc)).OnComplete(() => GetEXP(ExpAmount));
    }

    public void GetEXP(float value)
    {
        LevelManager.Instance.GetExp(value);
        ItemManager.Instance.TakeItem(this);
    }
}
