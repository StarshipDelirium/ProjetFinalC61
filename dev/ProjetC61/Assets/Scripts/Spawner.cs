using UnityEngine;

public class Spawner : MonoBehaviour
{
  public enum Animation
  {
    Idle,
    Summon,
    Destruct,
  }

  private Animation _currentAnimation;

  public Animation CurrentAnimation
  {
    get { return _currentAnimation; }
    set
    {
      _currentAnimation = value;
      UpdateAnimations();
    }
  }

  public string AnimationName
  {
    get
    {
      var suffix = CurrentAnimation.ToString();

      return "Spawner_" + suffix;
    }
  }

  private void UpdateAnimations()
  {
    var animation = AnimationName;

    Animator.Play(animation);
  }

  public float spawnTimer = 10;
  public Transform SpawnPoint;
  public Animator Animator;
  public Health Health;
  private SpriteRenderer Renderer;
  private void Awake()
  {
    Health = GetComponent<Health>();
    Animator = gameObject.GetComponent<Animator>();
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
    Renderer = gameObject.GetComponent<SpriteRenderer>();
    CurrentAnimation = Animation.Idle;

  }

  void Update()
  {
    if (Renderer.isVisible)                                                            // only spawn Skeletons if Spawner is on screen
    {
      spawnTimer -= Time.deltaTime;

      if (spawnTimer <= 0)
      {
        GameManager.Instance.PrefabManager.Spawn(PrefabManager.Enemy.Skeleton, SpawnPoint.position, transform.rotation);
        CurrentAnimation = Animation.Summon;
        spawnTimer = 5;
        GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Spawner);
      }
    }

  }

  private void OnHit(Health health)
  {
    //Play hit sound
    Flash flash = gameObject.GetComponent<Flash>();                 // if enemy is hit, will stop moving and flash for 1 second
    flash.Duration = 0.5f;
    flash.StartFlash();
  }

  private void OnDeath(Health health)
  {
    float LootChance = Random.Range(0.0f, 1.0f);

    if (LootChance < 0.40)
    {
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Usable.HealthElixir, gameObject.transform.position, gameObject.transform.rotation);              // 40% chance of dropping Health Elixir on death
    }
    else if (LootChance > 0.40 && LootChance < 0.80)
    {
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Usable.ManaPotion, gameObject.transform.position, gameObject.transform.rotation);                // 40% chance Mana Elixir
    }
    else
    {
      Vector3 dropPosition = gameObject.transform.position;                                                                                                   // 20% chance of dropping both elixirs
      float xHE = dropPosition.x - 0.5f;
      float xME = dropPosition.x + 0.5f;

      dropPosition.x = xHE;
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Usable.HealthElixir, dropPosition, gameObject.transform.rotation);

      dropPosition.x = xME;
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Usable.ManaPotion, dropPosition, gameObject.transform.rotation);
    }

    CurrentAnimation = Animation.Destruct;
  }

  public void OnDestructComplete()
  {
    Destroy(gameObject);                                        // called on last frame of Destruct animation
  }
}
