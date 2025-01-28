using UnityEngine;
using System.Collections;
public abstract class  Item : MonoBehaviour
{
    public string ItemName;
    private void OnEnable()
    {
        StartCoroutine(DisappearAfter3s());
    }

    private IEnumerator DisappearAfter3s()
    {
        yield return new WaitForSeconds(3f);
        UseItem();
    }

    public abstract void UseItem();
}
