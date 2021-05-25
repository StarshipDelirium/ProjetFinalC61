using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
  public Slider HealthBar;
  public Health playerHealth;
  private void Start()
  {
    playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
    HealthBar = gameObject.GetComponent<Slider>();
    HealthBar.maxValue = playerHealth.Max;
    HealthBar.value = playerHealth.Value;                                         // Setting HP to player current health value to avoid maxing health on scene change
    playerHealth.OnChanged += OnHealthChanged;
  }

  private void OnHealthChanged(Health health)                                                     // using Health onChanged event to modify Health bar value dynamically
  {
    HealthBar.value = health.Value;
  }

  public int GetCurrentHealth()
  {
    return playerHealth.Value;
  }
}
