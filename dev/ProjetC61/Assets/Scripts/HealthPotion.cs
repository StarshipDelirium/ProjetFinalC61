﻿public class HealthPotion : Item
{
  private Health playerHealth;

  private void Awake()
  {
    playerHealth = GameManager.Instance.Player.GetComponent<Health>();
    Name = "Health Potion";
    ID = "HP";
    Description = "Restores 5 HP";
    Stats = 5;
    TotalCount = 0;
    IsConsumable = true;

  }


  public override void Use()
  {
    if (TotalCount > 0)
    {
      this.TotalCount -= 1;
      playerHealth.Value += Stats;
      GetComponent<InventorySlot>().Qty.text = TotalCount.ToString();
    }

    CheckCount();
  }
}