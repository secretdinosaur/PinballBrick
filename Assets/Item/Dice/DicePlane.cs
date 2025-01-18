using UnityEngine;

public class DicePlane : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        switch (col.gameObject.name)
        {
            case "DiceFace1":
                col.GetComponentInParent<Dice>().Die = 6;
                break;
            case "DiceFace2":
                col.GetComponentInParent<Dice>().Die = 5;
                break;
            case "DiceFace3":
                col.GetComponentInParent<Dice>().Die = 4;
                break;
            case "DiceFace4":
                col.GetComponentInParent<Dice>().Die = 3;
                break;
            case "DiceFace5":
                col.GetComponentInParent<Dice>().Die = 2;
                break;
            case "DiceFace6":
                col.GetComponentInParent<Dice>().Die = 1;
                break;
        }
    }
}
