using UnityEngine;

public class Possessed : MonoBehaviour
{
  public enum Animation
  {
    Death,
    Idle,
    Vomit,
    Walk,
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
  public float AttackDelay;
  public BoxCollider2D bossCollider;
  public MovementController MovementController;
  public LevelBoss LevelBoss;
  public Health Health { get; private set; }
  public SpriteRenderer Renderer;
  public float StunnedTimer = 1.5f;
  public float AttackRange;
  public bool playerInRange = false;
  public BoxCollider2D playerCollider;
  private Vector3 StartPosition;
  private Player player;
  public float NormalSpeed;


  private bool isHit;
  private bool isOnScreen = false;

  private void Awake()
  {
    Animator = GetComponent<Animator>();
    MovementController = GetComponent<MovementController>();
    NormalSpeed = MovementController.MoveSpeed;
    Renderer = GetComponent<SpriteRenderer>();
    Health = GetComponent<Health>();
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
    player = GameManager.Instance.Player;
    playerCollider = player.GetComponent<BoxCollider2D>();
    bossCollider = GetComponent<BoxCollider2D>();
    FacingController = GetComponent<FacingController>();
    FacingController.Facing = Facing.Left;
    CurrentAnimation = Animation.Idle;
  }

  void Update()
  {


    if (!isOnScreen && Renderer.isVisible)                                                                                            // since spawned off screen, wait until player enters area before moving
    {
      isOnScreen = true;
      GameManager.Instance.Camera.GetComponent<FollowObject>().OnTriggerBossFight();                                          // stop camera from following player to enclose stage
      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.BossScream);
      GameManager.Instance.SoundManager.Play(SoundManager.Music.RequiemForTheBeast);


    }

    if (isOnScreen)
    {
      var playerPositionX = playerCollider.bounds.max.x;
      var bossPositionX = bossCollider.bounds.max.x;
      float xDelta = Mathf.Abs(playerPositionX - bossPositionX);

      if (CurrentAnimation != Animation.Vomit && Mathf.Floor(xDelta) > AttackRange)                                                    // determine if player close enough to trigger attack
      {
        playerInRange = false;
        CurrentAnimation = Animation.Walk;
      }
      else
      {
        playerInRange = true;

      }

      if (CurrentAnimation == Animation.Walk)
      {
        MovementController.InputMove = MovementController.FacingController.Direction;

      }

      if (playerCollider.bounds.max.x < bossCollider.bounds.min.x)                     // adjust facing depending on player position
      {
        FacingController.Facing = Facing.Left;
      }
      else if (playerCollider.bounds.min.x > bossCollider.bounds.max.x)
      {
        FacingController.Facing = Facing.Right;
      }


      if (AttackDelay > 0)
      {
        AttackDelay -= Time.deltaTime;
      }
      else if (AttackDelay <= 0 && playerInRange)
      {
        CurrentAnimation = Animation.Vomit;
        GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Vomit);
        AttackDelay = 5;
      }

      if (isHit && StunnedTimer > 0)
      {
        StunnedTimer -= Time.deltaTime;

      }
      else if (isHit && StunnedTimer <= 0)
      {
        isHit = false;
        StunnedTimer = 1.5f;
        Animator.enabled = true;
        MovementController.MoveSpeed = NormalSpeed;
      }
    }
  }

  public void OnHit(Health health)
  {
    Debug.Log("HEALTH: " + Health.Value);
    MovementController.MoveSpeed = 0.0f;
    Flash flash = gameObject.GetComponent<Flash>();                 // if enemy is hit, will stop moving and flash for 1 second
    flash.Duration = 1.5f;
    flash.StartFlash();
    isHit = true;
    Animator.enabled = false;
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.BossHit);
  }

  public void OnDeath(Health health)
  {
    CurrentAnimation = Animation.Death;
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.BossScream);

  }

  public void OnDeathComplete()
  {
    GameManager.Instance.Camera.GetComponent<FollowObject>().OnBossKilled();                                                // resume camera follow
    Fade fade = gameObject.AddComponent<Fade>();
    fade.FadeOutTime = 2;
    fade.StartFade();
    fade.DestroyOnFadeOut = true;
  }

  public void OnAttackComplete()
  {
    CurrentAnimation = Animation.Idle;
  }

}
