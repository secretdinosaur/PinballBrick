using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Pin : MonoBehaviour
{
    public float RotationSpeed;
    public float TargetAngle;
    public float CurrentAngle;

    private bool isButtonPressed = false;
    private float BeginningYAngle;
    private float BeginningZAngle;
    private Rigidbody2D rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BeginningYAngle = transform.rotation.eulerAngles.y;
        BeginningZAngle = transform.rotation.eulerAngles.z;
    }
    public void Update()
    {
        CurrentAngle = GetTrueAngle(transform.rotation.eulerAngles.z);
    }
    public void FixedUpdate()
    {
        if (isButtonPressed)
        {
            if (CurrentAngle < TargetAngle)
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
            if (CurrentAngle > BeginningZAngle)
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
        if(angle > 180)
        {
            return angle - 360;
        }
        return angle;
    }
}