public class ManaPotion : Item
{

  private Mana playerMana;

  private void Start()
  {
    playerMana = GameManager.Instance.Player.GetComponent<Mana>();
    Name = "Mana Potion";
    ID = "MP";
    Description = "Restores 5 mana";
    Stats = 5;
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
