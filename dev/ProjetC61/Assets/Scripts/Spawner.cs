using UnityEngine;

public class Spawner : SimpleEnemy
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

  public float spawnTimer = 5;
  public Transform SpawnPoint;
  public Animator Animator;
  private void Awake()
  {
    Animator = gameObject.GetComponent<Animator>();
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
}
