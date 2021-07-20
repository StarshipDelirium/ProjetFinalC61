public class ManaPotion : Item
{

  public Mana playerMana;

  private void Awake()
  {
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
      playerMana = FindObjectOfType<Player>().GetComponent<Mana>();
      playerMana.Value += Stats;
      gameObject.GetComponent<InventorySlot>().Qty.text = TotalCount.ToString();
      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.ManaRegen);
    }

    CheckCount();
  }
}
