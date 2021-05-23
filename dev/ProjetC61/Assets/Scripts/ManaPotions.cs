public class ManaPotions : Item
{

  public int mana;

  private void Awake()
  {
    mana = gameObject.CompareTag("ManaPotion") ? 10 : 20;
  }
  public override void Use()
  {
    // Remove 1 from inventory
    this.TotalCount -= 1;
    GameManager.Instance.Player.GetComponent<Mana>().Value += mana;
  }

}
