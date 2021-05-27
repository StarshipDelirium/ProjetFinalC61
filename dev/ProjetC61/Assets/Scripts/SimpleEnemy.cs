using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Flash))]
public class SimpleEnemy : MonoBehaviour
{
  private int _damage;

  public int Damage                   // Damage dealt to player                           
  {
    get { return _damage; }
    set { _damage = value; }
  }

  public float StunnedTimer = 1;

  public Health Health { get; private set; }
  private Animator animator;
  private float normalSpeed;
  public bool isHit;
  private void Awake()
  {
    Health = GetComponent<Health>();
    animator = GetComponent<Animator>();
    isHit = false;
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
  }

  // Update is called once per frame
  void Update()
  {
    if (isHit && StunnedTimer > 0)
    {
      StunnedTimer -= Time.deltaTime;
    }
    else if (isHit && StunnedTimer <= 0)
    {
      isHit = false;
      StunnedTimer = 1;
      animator.enabled = true;
    }
  }

  private void OnHit(Health health)
  {
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Hit);
    Flash flash = gameObject.GetComponent<Flash>();                 // if enemy is hit, will stop moving and flash for 1 second
    flash.Duration = 1.0f;
    flash.StartFlash();
    isHit = true;
    animator.enabled = false;

  }

  private void OnDeath(Health health)
  {
    float LootChance = Random.Range(0.0f, 1.0f);

    if (LootChance < 0.10)
    {
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Usable.HealthPotion, gameObject.transform.position, gameObject.transform.rotation);              // 10% chance of dropping Health Potion on death
    }
    else if (LootChance > 0.95)
    {
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Usable.ManaPotion, gameObject.transform.position, gameObject.transform.rotation);                // 5% chance mana potion
    }

    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.EnemyDeath);

    Flash flash = gameObject.GetComponent<Flash>();
    flash.StartFlash();
    Fade fade = gameObject.AddComponent<Fade>();
    fade.FadeOutTime = 1;
    fade.StartFade();
    fade.DestroyOnFadeOut = true;
  }
}
