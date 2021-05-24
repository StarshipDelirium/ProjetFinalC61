public class CemeteryKey : Item
{
  private void Awake()
  {
    Name = "Cemetery Key";
    ID = "CK";
    Description = "A creepy old key";
    Stats = 0;
    IsConsumable = false;
  }
  public override void Use()
  {
  }
}
