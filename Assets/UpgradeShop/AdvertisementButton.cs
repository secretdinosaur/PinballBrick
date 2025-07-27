using UnityEngine;

public class AdvertisementButton : MonoBehaviour
{
    public void OnAdButtonClick(string award, int amount)
    {
        //advertise


        if (award == "Gold") StatManager.instance.GetGold(10000);
        if (award == "Cash") StatManager.instance.GetGold(10000);
    }
}
