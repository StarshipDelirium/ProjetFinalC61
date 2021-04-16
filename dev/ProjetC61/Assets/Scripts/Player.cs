using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
  public enum Animation
  {
    Idle,
    Jump,
    Run,
    Attack,
    Hurt,
    Crouch,
    Fall,
    Block,
    Climb,
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

  private int _currentDamage;                               // current damage dealt by player
  public int CurrentDamage
  {
    get { return _currentDamage; }
    set { _currentDamage = value; }
  }

  public Vector2 RunAnimationSpeed = new Vector2(0.05f, 0.35f);
  public MovementController MovementController { get; private set; }
  private BoxCollider2D playerCollider;
  public CapsuleCollider2D SwordCollider { get; private set; }
  public Animator Animator { get; private set; }
  private IEnumerator coroutine;
  //public Health Health { get; private set; }
  public bool isInvincible = false;
  public bool isRestart = false;
  public float invincibleTimer = 2;
  private float defaultSpeed;


  private void Awake()
  {
    MovementController = GetComponent<MovementController>();
    SwordCollider = GetComponentInChildren<CapsuleCollider2D>();
    playerCollider = gameObject.GetComponent<BoxCollider2D>();

    MovementController.OnJump += OnJump;
    MovementController.OnFall += OnFall;
    MovementController.OnMoveStart += OnMoveStart;
    MovementController.OnMoveStop += OnMoveStop;
    MovementController.OnLand += OnLand;

    defaultSpeed = MovementController.MoveSpeed;
    Animator = GetComponent<Animator>();
    CurrentAnimation = Animation.Idle;
    CurrentDamage = 1;
  }
  void Update()
  {

    MovementController.InputJump = Input.GetKey("space");

    MovementController.InputMove = Input.GetAxisRaw("Horizontal");



    if (Input.GetButtonDown("Fire1"))
    {
      Animator.SetTrigger("IsAttacking");
      MovementController.IsAttacking = true;
      CurrentAnimation = Animation.Attack;

      //GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Attack);
    }

    if (Input.GetKeyDown("left shift"))
    {
      CurrentAnimation = Animation.Block;
    }

    if (Input.GetKeyUp("left shift"))
    {
      CurrentAnimation = Animation.Idle;
    }

    if (Input.GetKeyDown("left ctrl"))
    {
      CurrentAnimation = Animation.Crouch;
    }

    if (Input.GetKeyUp("left ctrl"))
    {
      CurrentAnimation = Animation.Idle;
    }

    if (CurrentAnimation == Animation.Run)
    {
      var speedRatio = MovementController.CurrentSpeed / MovementController.MoveSpeed;
      Animator.speed = RunAnimationSpeed.Lerp(speedRatio);
    }
    else
    {
      Animator.speed = 1.0f;
    }

  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    var health = collision.GetComponentInParent<Health>();
    var playerPos = playerCollider.bounds.min.y;

    if (health)
    {
      var enemyPosition = collision.bounds.min.y + 0.7 * collision.bounds.extents.y;

      if (playerPos > enemyPosition)
      {
        health.Value -= 1;
        MovementController.Jump();
      }
      else
      { // Enemy Wins
        if (!isInvincible)
        {
          //Health.Value -= 1;
        }
      }
    }
  }
  private void OnJump(MovementController platform)
  {
    Animator.SetTrigger("IsJumping");
    CurrentAnimation = Animation.Jump;
    //GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Jump);
  }

  private void OnFall(MovementController platform)
  {
    CurrentAnimation = Animation.Fall;
  }

  private void OnMoveStart(MovementController platform)
  {
    if (MovementController.IsGrounded)
    {
      Animator.SetTrigger("IsRunning");
      CurrentAnimation = Animation.Run;
    }
  }

  private void OnMoveStop(MovementController platform)
  {
    if (MovementController.IsGrounded)
    {
      CurrentAnimation = Animation.Idle;
    }
  }

  private void OnLand(MovementController platform)
  {
    if (MovementController.IsMoving)
    {
      CurrentAnimation = Animation.Run;
    }
    else
    {
      CurrentAnimation = Animation.Idle;
    }
  }

  private IEnumerator AttackOnce()
  {
    Animator.SetBool("IsAttacking", true);
    yield return null;
    Animator.SetBool("IsAttacking", false);
  }

  public void OnLevelStart(LevelEntrance levelEntrance)
  {
    if (levelEntrance != null)
    {
      transform.position = levelEntrance.transform.position;
    }
    else
    {
      transform.position = Vector3.zero;
    }

    MovementController.Reset();
  }

  public void OnLevelRestart()
  {
    MovementController.BoxCollider2D.enabled = true;
    MovementController.enabled = true;
    //Health.Value = 1;
    GameManager.Instance.Camera.GetComponent<FollowObject>().TargetTransform = gameObject.transform;

  }

  private void Start()
  {
    OnLevelRestart();
  }

  public void OnAttackStart()                                                   // Fired on first frame of Attack Animation
  {
    MovementController.MoveSpeed = 0;                                           // Force player movement speed to 0 to avoid sliding while attacking
  }
  public void OnAttackComplete()                                                // Fired on last frame of Attack Animation
  {
    MovementController.MoveSpeed = defaultSpeed;                                // Resume normal movement speed and toggle Attack off
    MovementController.IsAttacking = false;

    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                     // Checks if player currently holding A or D key down to immediately switch to Run animation instead of toggling to default Idle
    {
      CurrentAnimation = Animation.Run;
    }
  }
}
