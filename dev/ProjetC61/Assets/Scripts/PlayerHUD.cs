﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
  public Slider HealthBar;
  public Health playerHealth;
  private void Start()
  {
    playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    HealthBar = gameObject.GetComponent<Slider>();
    HealthBar.maxValue = playerHealth.Value;
    HealthBar.value = playerHealth.Value;                                         // Setting HP to player current health value to avoid maxing health on scene change
    playerHealth.OnChanged += OnHealthChanged;


  }

  private void OnHealthChanged(Health health)                                                     // using Player onChanged event to modify Health bar value dynamically
  {
    HealthBar.value = health.Value;
  }
}
