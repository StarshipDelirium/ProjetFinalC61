﻿public class ManaElixir : Item
{
  private Mana playerMana;
  private void Awake()
  {
    playerMana = GameManager.Instance.Player.GetComponent<Mana>();
    Name = "Mana Elixir";
    ID = "ME";
    Description = "Fully restores mana";
    Stats = playerMana.Max;
    IsConsumable = true;
  }
  public override void Use()
  {
    if (TotalCount > 0)
    {
      this.TotalCount -= 1;
      playerMana.Value += Stats;
      GetComponent<InventorySlot>().Qty.text = TotalCount.ToString();
    }
    CheckCount();
  }
}