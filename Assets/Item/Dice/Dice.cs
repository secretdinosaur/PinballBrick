using UnityEngine;
using System.Collections;
public class Dice : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public int Die;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        transform.position = new Vector3(Random.Range(-1f, 1f), -3, -15);
        Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 5f, 5f), ForceMode.Impulse);
        Rigidbody.AddTorque(new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)) * 5f, ForceMode.Impulse);
    }
}
