using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Flash))]
public class SimpleEnemy : MonoBehaviour
{
  private int _damage;

  public int Damage
  {
    get { return _damage; }
    set { _damage = value; }
  }

  public Health Health { get; private set; }
  private void Awake()
  {
    Health = GetComponent<Health>();
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnHit(Health health)
  {
    //Play hit sound
    Flash flash = gameObject.GetComponent<Flash>();
    flash.Duration = 1.0f;
    flash.StartFlash();

  }

  private void OnDeath(Health health)
  {

    Flash flash = gameObject.GetComponent<Flash>();
    flash.StartFlash();
    Fade fade = gameObject.AddComponent<Fade>();
    fade.FadeOutTime = 1;
    fade.StartFade();

    // Destroy object or store in pool
    Destroy(gameObject);
  }
}
