using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Pin : MonoBehaviour
{
    public float RotationSpeed;
    public float TargetAngle;
    private bool isButtonPressed = false;
    public float CurrentAngle;
    private float BeginningYAngle;

    private float Angle;
    public void Start()
    {
        BeginningYAngle = transform.rotation.eulerAngles.y;
    }
    public void Update()
    {
        if (isButtonPressed)
        {
            if (CurrentAngle < TargetAngle)
            {
                //transform.Rotate(new Vector3(0, 0, RotationSpeed) * Time.deltaTime, Space.World);
                float Angle = RotationSpeed * Time.deltaTime;
                CurrentAngle += Angle;
                transform.rotation = Quaternion.Euler(0, BeginningYAngle, CurrentAngle);
            }
            else
            {
                //transform.eulerAngles = new Vector3(0, BeginningAngle.y, TargetAngle);
                transform.rotation = Quaternion.Euler(0, BeginningYAngle, TargetAngle);
            }
        }
        else
        {
            if (CurrentAngle > 0)
            {
                //transform.Rotate(new Vector3(0, 0, -RotationSpeed) * Time.deltaTime, Space.World);
                float Angle = -RotationSpeed * Time.deltaTime;
                CurrentAngle += Angle;
                transform.rotation = Quaternion.Euler(0, BeginningYAngle, CurrentAngle);
            }
            else
            {
                //transform.eulerAngles = new Vector3(0, BeginningAngle.y, BeginningAngle.z);
                transform.rotation = Quaternion.Euler(0, BeginningYAngle, 0);
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
}
