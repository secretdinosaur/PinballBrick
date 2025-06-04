using UnityEngine;
using DG.Tweening;
public class Gold3 : Item
{
    public int GoldAmount;
    public override void MoveItem()
    {
        Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane);
        Vector3 worldTopRight = Camera.main.ScreenToWorldPoint(screenTopRight);

        float angle = Random.Range(135f, 315f);
        Vector3 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(transform.position + direction * Random.Range(0.08f, 0.12f), Random.Range(0.3f, 0.7f)).SetEase(Ease.OutCirc));
        seq.Append(transform.DOMove(worldTopRight, 0.5f).SetEase(Ease.InCirc));
        StatManager.instance.GetGold(GoldAmount);
        ItemManager.Instance.TakeItem(this);
    }
}
