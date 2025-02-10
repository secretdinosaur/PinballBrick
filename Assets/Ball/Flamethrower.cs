using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float FlameDamage = 0.3f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.GetComponent<IBrick>().ArbitraryDamageAttack(FlameDamage, false, FlameDamage.ToString("F1"), "Fire");
        }
    }

    public void FlamethrowerOn()
    {
        gameObject.SetActive(true);
    }
}
