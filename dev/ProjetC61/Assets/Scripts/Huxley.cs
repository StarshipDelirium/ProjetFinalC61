using UnityEngine;

public class Huxley : MonoBehaviour
{
  public enum Direction
  {
    DiagDownLeft,
    DiagDownRight,
    DiagUpLeft,
    DiagUpRight,
    Down,
    Left,
    Right,
    Up,
  }

  public enum Animation
  {
    Dead,
    Idle,
    Run,
    Walk,
  }

  private Direction _currentDirection;
  public Direction CurrentDirection
  {
    get { return _currentDirection; }
    set
    {
      _currentDirection = value;
      UpdateAnimations();
    }
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
      var prefix = CurrentDirection.ToString();
      var suffix = CurrentAnimation.ToString();
      return "Huxley_" + prefix + "_" + suffix;

    }
  }

  private void UpdateAnimations()
  {
    var animation = AnimationName;
    Animator.Play(animation);
  }

  public Vector2 RunAnimationSpeed = new Vector2(0.05f, 0.35f);
  //public Transform FireballSpawnPoint;
  public MovementController MovementController { get; private set; }
  public Animator Animator { get; private set; }
  public Camera Camera;
  public Rigidbody2D playerPos;
  public bool isRunning;
  //public Health Health { get; private set; }
  private Vector2 posDelta;
  private Vector2 mousePos;
  private Vector2 movement;

  void Awake()
  {
    MovementController = GetComponent<MovementController>();
    Camera = GameManager.Instance.Camera;
    playerPos = GetComponent<Rigidbody2D>();

    //FireballSpawnPoint = GetComponentInChildren<Transform>();
    MovementController.OnMoveStart += OnMoveStart;
    MovementController.OnMoveStop += OnMoveStop;
    // Health = GetComponent<Health>();
    //Health.OnDeath += OnDeath;
    //Health.OnChanged += OnChanged;

    Animator = GetComponent<Animator>();

    CurrentDirection = Direction.Left;
    CurrentAnimation = Animation.Idle;
    isRunning = false;
  }

  // Update is called once per frame
  void Update()
  {
    MovementController.InputMove = Input.GetAxisRaw("Horizontal");

    float inputX = Input.GetAxisRaw("Horizontal");
    float inputY = Input.GetAxisRaw("Vertical");
    mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);

    posDelta = mousePos - playerPos.position;

    movement = new Vector2(inputX, inputY);

    if (mousePos.x > playerPos.position.x)
    {
      CurrentDirection = Direction.Right;
    }
    else
    {
      CurrentDirection = Direction.Left;
    }


    // Diagonals

    /*if (posDelta.x != 0 && posDelta.y != 0)
    {
      if (posDelta.y == 1 && posDelta.x == -1)
      {
        CurrentDirection = Direction.DiagUpLeft;
      }
      else if (posDelta.y == 1 && posDelta.x == 1)
      {
        CurrentDirection = Direction.DiagUpRight;
      }
      else if (posDelta.y == -1 && posDelta.x == -1)
      {
        CurrentDirection = Direction.DiagDownLeft;
      }
      else if (posDelta.y == -1 && posDelta.x == 1)
      {
        CurrentDirection = Direction.DiagDownRight;
      }
    }
    else
    {
      if (posDelta.x == -1)
      {
        CurrentDirection = Direction.Left;
      }
      else if (posDelta.x == 1)
      {
        CurrentDirection = Direction.Right;
      }
      else if (posDelta.y == 1)
      {
        CurrentDirection = Direction.Up;
      }
      else if (posDelta.y == -1)
      {
        CurrentDirection = Direction.Down;
      }
    }*/

    if (Input.GetButtonDown("Fire1"))
    {
      // Huxley shoot
      // spawn bullet
    }
    else if (Input.GetKey("left shift"))
    {
      isRunning = true;
    }

    if (CurrentAnimation == Animation.Run)
    {
      //var speedRatio = MovementController.CurrentSpeed / MovementController.MoveSpeed;
      //Animator.speed = RunAnimationSpeed.Lerp(speedRatio);
      Animator.speed = 2.0f;
    }
    else
    {
      Animator.speed = 1.0f;
    }
  }

  private void OnMoveStart(MovementController controller)
  {
    if (isRunning)
    {
      CurrentAnimation = Animation.Run;
    }
    else
    {
      CurrentAnimation = Animation.Walk;
    }

  }

  private void OnMoveStop(MovementController platform)
  {
    CurrentAnimation = Animation.Idle;
    isRunning = false;
  }

  /*private void OnDeath(Health health)
  {
    GameManager.Instance.Camera.GetComponent<FollowObject>().TargetTransform = null;
    CurrentAnimation = Animation.Dead;
    PlatformController.enabled = false;
    PlatformController.BoxCollider2D.enabled = false;

    isRestart = true;
    GameManager.Instance.Invoke(nameof(GameManager.RestartLevel), 3.0f);
  }

  private void OnChanged(Health health)
  {
    if (!isRestart && health.Value == 1)
    {
      CurrentState = State.Small;
      isInvincible = true;
      Flash flash = gameObject.GetComponent<Flash>();
      flash.StartFlash();
      GameManager.Instance.SoundManager.Play(SoundManager.PlatformerSfx.Hit);
    }
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

    PlatformController.Reset();
  }
  public void OnLevelRestart()
  {
    PlatformController.BoxCollider2D.enabled = true;
    PlatformController.enabled = true;
    Health.Value = 1;
    CurrentState = State.Small;
    GameManager.Instance.Camera.GetComponent<FollowObject>().TargetTransform = gameObject.transform;

    isRestart = false;
  }

  private void Start()
  {
    OnLevelRestart();
  }*/
}
