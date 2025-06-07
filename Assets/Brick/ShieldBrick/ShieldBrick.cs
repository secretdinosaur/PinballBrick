using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;
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
        base.OnEnable();
        BrickType = "ShieldBrick";
        Shield.SetActive(true);
        Shield.GetComponent<Renderer>().material.SetFloat("_Alpha", 0);
        IsShieldDestroyed = false;
        ShieldCount = BasicShieldCount * BrickManager.Instance.HPPool[Random.Range(0, BrickManager.Instance.HPPool.Count)] / 20;
        HPMultiplier = ShieldCount / BasicShieldCount;
        HP = ShieldCount;
        HpTemp = HP;
        HPText.enabled = true;
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
        StartCoroutine(DestroySheildEffect());
        
    }

    private IEnumerator DestroySheildEffect()
    {
        float elapsedTime = 0f;
        while(elapsedTime < 0.5f)
        {
            Shield.GetComponent<Renderer>().material.SetFloat("_Alpha", Mathf.Lerp(0, 1, elapsedTime / 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Shield.SetActive(false);

        IsShieldDestroyed = true;
        HP = (IsBoss) ? BasicHP * HPMultiplier : BasicHP * HPMultiplier * 10;
        HpTemp = HP;
        HPText.enabled = true;
    }
}
