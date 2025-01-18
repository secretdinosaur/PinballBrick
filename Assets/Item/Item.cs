using UnityEngine;
using System.Collections;
public abstract class Item : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisappearAfter3s());
    }

    private IEnumerator DisappearAfter3s()
    {
        yield return new WaitForSeconds(3f);
        UseItem();
        ItemManager.Instance.TakeItem(this);
    }

    public abstract void UseItem();
}
