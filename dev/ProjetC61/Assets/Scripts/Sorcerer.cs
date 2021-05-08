using UnityEngine;
public class Sorcerer : SimpleEnemy
{
  public enum Animation
  {
    Idle,
    Casting,
    Fireball,
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

      return suffix;
    }
  }

  private void UpdateAnimations()
  {
    var animation = AnimationName;

    Animator.Play(animation);
  }

  public Animator Animator;
  public FacingController FacingController;
  public Transform LeftFireballSpawnPoint;
  public Transform RightFireballSpawnPoint;
  public float FireballDelay = 6;
  public SorcererFireball fireballPrefab;
  private void Awake()
  {
    Animator = GetComponent<Animator>();
    FacingController = GetComponent<FacingController>();
    FacingController.Facing = Facing.Left;
    CurrentAnimation = Animation.Idle;
  }

  // Update is called once per frame
  void Update()
  {
    if (FireballDelay > 5)
    {
      CurrentAnimation = Animation.Idle;
      FireballDelay -= Time.deltaTime;
    }
    else if (FireballDelay <= 0)
    {
      CurrentAnimation = Animation.Fireball;
      FireballDelay = 6;

      if (FacingController.Facing == Facing.Left)
      {
        Quaternion rotation = new Quaternion(0f, -90.0f, 0.0f, 0.0f);
        PoolManager.Spawn(GameManager.Instance.PrefabManager.Spawn(PrefabManager.Projectiles.SorcererFireball), LeftFireballSpawnPoint.position, rotation);

      }
      else
      {
        Quaternion rotation = new Quaternion(0f, 90.0f, 0.0f, 0.0f);
        PoolManager.Spawn(GameManager.Instance.PrefabManager.Spawn(PrefabManager.Projectiles.SorcererFireball), RightFireballSpawnPoint.position, rotation);
      }
    }
    else
    {
      CurrentAnimation = Animation.Casting;
      FireballDelay -= Time.deltaTime;
    }
  }
}
