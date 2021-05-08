using UnityEngine;

public class PlayerController : MonoBehaviour
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

  public Vector2 RunAnimationSpeed = new Vector2(0.05f, 0.35f);
  public Animator Animator;
  public MovementController MovementController;
  private float defaultSpeed;
  private void Awake()
  {
    Animator = gameObject.GetComponent<Animator>();
    MovementController = gameObject.GetComponent<MovementController>();
    MovementController.OnJump += OnJump;
    MovementController.OnFall += OnFall;
    MovementController.OnMoveStart += OnMoveStart;
    MovementController.OnMoveStop += OnMoveStop;
    MovementController.OnLand += OnLand;
    defaultSpeed = MovementController.MoveSpeed;
  }

  // Update is called once per frame
  void Update()
  {

    if (Input.GetButtonDown("Fire1"))
    {
      Animator.SetTrigger("IsAttacking");
      MovementController.IsAttacking = true;
      CurrentAnimation = Animation.Attack;

      //GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Attack);
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

    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))                     // Checks if player currently holding A or D key down to immediately switch to Run animation instead of toggling to default Idle
    {
      CurrentAnimation = Animation.Run;
    }
  }
}
