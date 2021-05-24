public class HealthElixir : Item
{
  private Health playerHealth;

  private void Awake()
  {
    playerHealth = GameManager.Instance.Player.GetComponent<Health>();
    Name = "Health Elixir";
    ID = "HE";
    Description = "Fully restores health";
    Stats = playerHealth.Max;
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
