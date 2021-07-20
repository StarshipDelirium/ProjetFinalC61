public class HealthPotion : Item
{
  public Health playerHealth;

  private void OnEnable()
  {

    Name = "Health Potion";
    ID = "HP";
    Description = "Restores 5 HP";
    Stats = 5;
    IsConsumable = true;

  }


  public override void Use()
  {
    if (TotalCount > 0)
    {

      this.TotalCount -= 1;
      playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
      playerHealth.Value += Stats;
      GetComponent<InventorySlot>().Qty.text = TotalCount.ToString();
      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.HealthRegen);
    }

    CheckCount();
  }
}
