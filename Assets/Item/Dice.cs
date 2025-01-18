using UnityEngine;
using System.Collections;
public class Dice : MonoBehaviour
{
    public Rigidbody Rigidbody;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), 5f, 3f), ForceMode.Impulse);
        Rigidbody.AddTorque(new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)) * 10f, ForceMode.Impulse);
    }

}
