using UnityEngine;

public class Spawner : MonoBehaviour
{
  public enum Animation
  {
    Idle,
    Summon,
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
  private void Awake()
  {
    Health = GetComponent<Health>();
    Animator = gameObject.GetComponent<Animator>();
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
    CurrentAnimation = Animation.Idle;

  }

  void Update()
  {
    spawnTimer -= Time.deltaTime;

    if (spawnTimer <= 0)
    {
      GameManager.Instance.PrefabManager.Spawn(PrefabManager.Enemy.Skeleton, SpawnPoint.position, transform.rotation);
      CurrentAnimation = Animation.Summon;
      spawnTimer = 5;
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

    Flash flash = gameObject.GetComponent<Flash>();
    flash.StartFlash();
    Fade fade = gameObject.AddComponent<Fade>();
    fade.FadeOutTime = 1;
    fade.StartFade();

    // Destroy object or store in pool
    Destroy(gameObject);
  }
}
