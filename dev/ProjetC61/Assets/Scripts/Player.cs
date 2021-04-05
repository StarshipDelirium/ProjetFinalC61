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
      //var prefix = CurrentDirection.ToString();
      var suffix = CurrentAnimation.ToString();

      return suffix;
    }
  }

  private void UpdateAnimations()
  {
    var animation = AnimationName;
    //Animator.Play(animation);
    Animator.Play(animation);
  }

  public Vector2 RunAnimationSpeed = new Vector2(0.05f, 0.35f);
  public MovementController MovementController { get; private set; }
  public CapsuleCollider2D SwordCollider { get; private set; }
  public Animator Animator { get; private set; }
  private IEnumerator coroutine;
  //public Health Health { get; private set; }
  public bool isInvincible = false;
  public bool isRestart = false;
  public float invincibleTimer = 2;

  private void Awake()
  {
    MovementController = GetComponent<MovementController>();
    SwordCollider = GetComponentInChildren<CapsuleCollider2D>();
    MovementController.OnJump += OnJump;
    MovementController.OnFall += OnFall;
    MovementController.OnMoveStart += OnMoveStart;
    MovementController.OnMoveStop += OnMoveStop;
    MovementController.OnLand += OnLand;

    Animator = GetComponent<Animator>();
    CurrentAnimation = Animation.Idle;
  }
  void Update()
  {
    MovementController.InputJump = Input.GetButtonDown("Jump");
    MovementController.InputMove = Input.GetAxisRaw("Horizontal");

    if (Input.GetButtonDown("Fire1"))
    {

      CurrentAnimation = Animation.Attack;
      //GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Attack);
      //coroutine = AttackOnce();
      //StartCoroutine(coroutine);
    }

    if (Input.GetKeyDown("left shift"))
    {
      CurrentAnimation = Animation.Crouch;
    }

    if (Input.GetKeyUp("left shift"))
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

    if (CurrentAnimation == Animation.Attack)
    {
      SwordCollider.transform.localScale = gameObject.transform.localScale;
    }

  }

  private void OnJump(MovementController platform)
  {
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
    //CurrentState = State.Small;
    GameManager.Instance.Camera.GetComponent<FollowObject>().TargetTransform = gameObject.transform;

    //isRestart = false;
  }

  private void Start()
  {
    OnLevelRestart();
  }
}
