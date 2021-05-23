using UnityEngine;
using UnityEngine.UI;

public class HealthPotions : Item
{

  public int hp;
  private Health playerHealth = GameManager.Instance.Player.GetComponent<Health>();

  private void Awake()
  {
    if (gameObject.CompareTag("HealthPotion"))
    {
      Name = "Health Potion";
      Description = "Restores 20 HP";
      Icon = Resources.Load("items/health_potion.png") as Image;
      hp = 20;
    }
    else
    {
      Name = "Health Elixir";
      Description = "Fully restores health";
      Icon = Resources.Load("items/health_elixir.png") as Image;
      hp = playerHealth.Max;
    }

    TotalCount = 0;

  }
  public override void Use()
  {
    // Remove 1 from inventory
    this.TotalCount -= 1;
    playerHealth.Value += hp;
  }

}
