using UnityEngine;
using DG.Tweening;
public class Gold2 : Item
{
    public int GoldAmount;
    public override void MoveItem()
    {
        Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane);
        Vector3 worldTopRight = Camera.main.ScreenToWorldPoint(screenTopRight);

        float angle = Random.Range(135f, 315f);
        Vector3 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOMove(transform.position + direction * 0.1f, 0.5f).SetEase(Ease.OutCirc));
        seq.Append(transform.DOMove(worldTopRight, 0.5f).SetEase(Ease.InCirc));
        StatManager.instance.GetGold(GoldAmount);
        ItemManager.Instance.TakeItem(this);
    }
}
