using UnityEngine;

public class PausableSoundObject : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().pitch = 1f;
    }
}
