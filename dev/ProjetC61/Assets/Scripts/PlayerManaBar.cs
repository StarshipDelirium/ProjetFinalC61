using UnityEngine;
using UnityEngine.UI;

public class PlayerManaBar : MonoBehaviour
{
  public Slider ManaBar;
  public Mana playerMana;
  private void Start()
  {
    playerMana = FindObjectOfType<Player>().GetComponent<Mana>();
    ManaBar = gameObject.GetComponent<Slider>();
    ManaBar.maxValue = playerMana.Max;
    ManaBar.value = playerMana.Value;                                         // Setting MP to player current mana value to avoid maxing health on scene change
    playerMana.OnChanged += OnManaChanged;
  }

  private void OnManaChanged(Mana mana)                                                     // using Health onChanged event to modify Health bar value dynamically
  {
    ManaBar.value = mana.Value;
  }

  public int GetCurrentMana()
  {
    return playerMana.Value;
  }
}
