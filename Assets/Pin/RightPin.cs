using UnityEngine;
using UnityEngine.EventSystems;

public class RightPin : MonoBehaviour
{
    public float RotationSpeed;         // ´ÜÀ§: degrees per second
    public float TargetAngle;
    public float CurrentAngle;

    private bool isButtonPressed = false;
    private float BeginningYAngle;
    private float BeginningZAngle;
    private Rigidbody2D rb;

    private float fixedDeltaTime => Time.fixedDeltaTime;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BeginningYAngle = transform.rotation.eulerAngles.y;
        BeginningZAngle = transform.rotation.eulerAngles.z;
    }

    public void FixedUpdate()
    {
        CurrentAngle = GetTrueAngle(transform.rotation.eulerAngles.z);

        float deltaAngle = -RotationSpeed * fixedDeltaTime;

        if (isButtonPressed)
        {
            if (CurrentAngle + deltaAngle < TargetAngle)
            {
                rb.angularVelocity = RotationSpeed;
            }
            else
            {
                rb.angularVelocity = 0;
                transform.rotation = Quaternion.Euler(0, BeginningYAngle, TargetAngle);
            }
        }
        else
        {
            if (CurrentAngle - deltaAngle > BeginningZAngle)
            {
                rb.angularVelocity = -RotationSpeed;
            }
            else
            {
                rb.angularVelocity = 0;
                transform.rotation = Quaternion.Euler(0, BeginningYAngle, BeginningZAngle);
            }
        }
    }

    public void OnPointerDown()
    {
        isButtonPressed = true;
    }

    public void OnPointerUp()
    {
        isButtonPressed = false;
    }

    public float GetTrueAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
        {
            return angle - 360;
        }
        return angle;
    }
}
