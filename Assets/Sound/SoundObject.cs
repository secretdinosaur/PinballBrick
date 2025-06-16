using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().pitch = 1f;
    }
}
