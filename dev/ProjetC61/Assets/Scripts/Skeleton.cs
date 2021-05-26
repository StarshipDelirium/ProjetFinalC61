using UnityEngine;

public class Skeleton : SimpleEnemy
{
  public enum Animation
  {
    Default,          // Rise
    Action,           // Walk
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
  public BoxCollider2D skeletonCollider;
  public MovementController MovementController;
  public FacingController FacingController;
  public Player Player;
  public float NormalSpeed;
  private SpriteRenderer Renderer;
  private void Awake()
  {
    Animator = gameObject.GetComponent<Animator>();
    skeletonCollider = gameObject.GetComponent<BoxCollider2D>();
    MovementController = gameObject.GetComponent<MovementController>();
    FacingController = gameObject.GetComponent<FacingController>();
    Renderer = gameObject.GetComponent<SpriteRenderer>();
    FacingController.Facing = Facing.Left;
    NormalSpeed = MovementController.MoveSpeed;
    Player = GameManager.Instance.Player;
    CurrentAnimation = Animation.Default;
  }

  void Update()
  {
    if (!Renderer.isVisible)                                                            // destroy object once it is off screen
    {
      Destroy(gameObject);
    }

    if (CurrentAnimation == Animation.Action)
    {
      MovementController.InputMove = MovementController.FacingController.Direction;
    }

    if (isHit)
    {
      MovementController.MoveSpeed = 0.0f;
    }
    else
    {
      MovementController.MoveSpeed = NormalSpeed;
    }
  }

  public void StartWalk()
  {
    CurrentAnimation = Animation.Action;
  }

}
