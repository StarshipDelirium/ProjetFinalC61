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
    PowerAttack,
    Hurt,
    Crouch,
    Fall,
    Block,
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

  public Vector2 RunAnimationSpeed = new Vector2(0.05f, 0.35f);
  public Animator Animator;
  public MovementController MovementController;
  private float defaultSpeed;
  private int _currentDamage;                               // current damage dealt by player
  private float clickTime;
  private bool mouseClicked = false;
  public int CurrentDamage
  {
    get { return _currentDamage; }
    set { _currentDamage = value; }
  }
  //public MovementController MovementController { get; private set; }
  private BoxCollider2D playerCollider;
  public CapsuleCollider2D SwordCollider { get; private set; }
  //public Animator Animator { get; private set; }
  public Health Health;
  public Flash Flash;
  //public Health Health { get; private set; }
  public bool isInvincible = false;
  public bool isRestart = false;
  public float invincibleTimer = 2;
  //private IEnumerator coroutine;




  private void Awake()
  {
    //MovementController = GetComponent<MovementController>();
    SwordCollider = GetComponentInChildren<CapsuleCollider2D>();
    playerCollider = gameObject.GetComponent<BoxCollider2D>();
    Flash = gameObject.GetComponent<Flash>();
    Health = GetComponent<Health>();
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
    Health.OnChanged += OnChanged;
    Animator = gameObject.GetComponent<Animator>();
    MovementController = gameObject.GetComponent<MovementController>();
    MovementController.OnJump += OnJump;
    MovementController.OnFall += OnFall;
    MovementController.OnMoveStart += OnMoveStart;
    MovementController.OnMoveStop += OnMoveStop;
    MovementController.OnLand += OnLand;
    defaultSpeed = MovementController.MoveSpeed;
    CurrentAnimation = Animation.Idle;
    CurrentDamage = 1;
  }

  void Update()
  {

    MovementController.InputJump = Input.GetKey("space");

    MovementController.InputMove = Input.GetAxisRaw("Horizontal");

    if (Input.GetMouseButtonDown(0))
    {
      clickTime = Time.time;                                                    // Time saved until mouse button released to differentiate single click vs mouse hold
      mouseClicked = true;
    }

    if (Input.GetMouseButtonUp(0) && !MovementController.IsJumping && !MovementController.IsFalling)
    {
      Animator.SetTrigger("IsAttacking");
      MovementController.IsAttacking = true;

      if ((Time.time - clickTime < 0.25f))                                              // normal attack
      {
        CurrentDamage = 1;
        CurrentAnimation = Animation.Attack;
      }
      else if (mouseClicked && (Time.time - clickTime) > 0.25f)                         // mouse button held down for power attack
      {
        CurrentDamage = 3;
        CurrentAnimation = Animation.PowerAttack;

      }
    }



    /*if (Input.GetButtonDown("Fire1"))
    {
      Animator.SetTrigger("IsAttacking");
      MovementController.IsAttacking = true;
      CurrentAnimation = Animation.Attack;

      //GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Attack);
    }*/

    if (CurrentAnimation == Animation.Run)
    {
      var speedRatio = MovementController.CurrentSpeed / MovementController.MoveSpeed;
      Animator.speed = RunAnimationSpeed.Lerp(speedRatio);
    }
    else
    {
      Animator.speed = 1.0f;
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

    if (isInvincible && invincibleTimer > 0)
    {
      invincibleTimer -= Time.deltaTime;
    }
    else if (isInvincible && invincibleTimer <= 0)
    {
      isInvincible = false;
      invincibleTimer = 2;
      //Flash.StopFlash();
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    var transform = collision.GetComponentInParent<Transform>();
    if (!isInvincible)
    {
      StartCoroutine(Knockback(transform));
      Health.Value -= 1;
    }
  }

  private void OnChanged(Health health)
  {
    // might only be needed in UI
  }

  private void OnHit(Health health)
  {
    //Flash.StartFlash();
    CurrentAnimation = Animation.Hurt;

    isInvincible = true;
  }

  private void OnDeath(Health health)
  {
    gameObject.SetActive(false);
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

  public void OnAttackStart()                                                   // Fired on first frame of Attack Animation
  {
    MovementController.MoveSpeed = 0;                                           // Force player movement speed to 0 to avoid sliding while attacking
  }
  public void OnAttackComplete()                                                // Fired on last frame of Attack Animation
  {
    MovementController.MoveSpeed = defaultSpeed;                                // Resume normal movement speed and toggle Attack off
    MovementController.IsAttacking = false;

    ResumePlayerControl();
  }

  IEnumerator Knockback(Transform collisionSource)
  {
    MovementController.enabled = false;
    MovementController.Rigidbody2D.velocity = new Vector2((transform.position.x - collisionSource.position.x) * 4, MovementController.Rigidbody2D.velocity.y * 6);    // knocks back player in opposite direction of collision

    yield return new WaitForSeconds(0.3f);                      // to keep player immobile in knockback position before resuming control

    MovementController.enabled = true;
    ResumePlayerControl();

  }

  private void ResumePlayerControl()
  {
    if (Input.GetMouseButton(0))
    {
      CurrentAnimation = Animation.Attack;
    }
    else
    {
      if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                     // Checks if player currently holding A or D key down to immediately switch to Run animation instead of toggling to default Idle
      {
        CurrentAnimation = Animation.Run;
      }
      else if (Input.GetKey(KeyCode.Space))
      {
        CurrentAnimation = Animation.Jump;
      }
      else
      {
        CurrentAnimation = Animation.Idle;
      }
    }
  }
}
