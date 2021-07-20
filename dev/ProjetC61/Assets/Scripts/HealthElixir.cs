﻿public class HealthElixir : Item
{
  public Health playerHealth;

  private void Awake()
  {

    Name = "Health Elixir";
    ID = "HE";
    Description = "Fully restores health";
    Stats = playerHealth.Max;
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
