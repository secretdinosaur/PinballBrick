using UnityEngine;
using TMPro;
public class ShopStatTest : MonoBehaviour
{
    public TMP_Text ShowStat; 
    private Stat stat;

    void Start()
    {
        stat = StatManager.instance.LoadStats();
    }

    public void AttackDamageUp()
    {
        stat.AttackDamage += 1;
    }
    public void AttackDamageDown()
    {
        stat.AttackDamage -= 1;
    }
    public void StartingBallUp()
    {
        stat.StartingBall += 1;
    }
    public void StartingBallDown()
    {
        stat.StartingBall -= 1;
    }
    public void BonusExpUp()
    {
        stat.BonusExp += 0.1f;
    }
    public void BonusExpDown()
    {
        stat.BonusExp -= 0.1f;
    }
    public void ItemDropChanceUp()
    {
        stat.ItemDropChance += 0.01f;
    }
    public void ItemDropChanceDown()
    {
        stat.ItemDropChance -= 0.01f;
    }
    public void CriticalChanceUp()
    {
        stat.CriticalChance += 0.01f;
    }
    public void CriticalChanceDown()
    {
        stat.CriticalChance -= 0.01f;
    }
    public void CriticalMultiplierUp()
    {
        stat.CriticalMultiplier += 0.01f;
    }
    public void CriticalMultiplierDown()
    {
        stat.CriticalMultiplier -= 0.01f;
    }
    public void loadstat()
    {
        stat = StatManager.instance.LoadStats();
    }
    public void savestat()
    {
        StatManager.instance.SaveStats(stat);
    }
    public void Update()
    {
        ShowStat.text = "AttackDamage : " + stat.AttackDamage.ToString() + "\n" +
                        "StartingBall : " + stat.StartingBall.ToString() + "\n" +
                        "BonusExp : " + stat.BonusExp.ToString() + "\n" +
                        "ItemDropChance : " + stat.ItemDropChance.ToString() + "\n" +
                        "CriticalChance : " + stat.CriticalChance.ToString() + "\n" +
                        "CriticalMultiplier : " + stat.CriticalMultiplier.ToString() + "\n";
    }
}
