using UnityEngine;

public class WhiteBrick : IBrick
{
    private void OnEnable()
    {
        base.OnEnable();
        BrickType = "WhiteBrick";
        GetComponents();
    }
}