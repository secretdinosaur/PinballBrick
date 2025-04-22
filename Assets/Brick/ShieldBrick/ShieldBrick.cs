using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ShieldBrick : IBrick
{
    public float BasicShieldCount;
    public float ShieldCount;
    public bool IsShieldDestroyed;
    public float HPMultiplier;
    public bool FirstShieldSpawn = true;

    public GameObject Shield;
    private void OnEnable()
    {
        BrickType = "ShieldBrick";
        Shield.SetActive(true);
        IsShieldDestroyed = false;
        ShieldCount = BasicShieldCount * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)] / 5;
        HPMultiplier = ShieldCount / BasicShieldCount;
        HP = ShieldCount;
        HpTemp = HP;

        GetComponents();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(collision.gameObject.GetComponent<IBall>());
        }
    }

    public void DestroyShield()
    {
        Shield.SetActive(false);
        IsShieldDestroyed = true;
        HP = (IsBoss) ? BasicHP * HPMultiplier : BasicHP * HPMultiplier * 10;
        HpTemp = HP;
        HPText.enabled = true;
    }
}
