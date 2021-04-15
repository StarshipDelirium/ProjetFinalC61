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
  public Player Player;
  public float NormalSpeed;
  private void Awake()
  {
    Animator = gameObject.GetComponent<Animator>();
    skeletonCollider = gameObject.GetComponent<BoxCollider2D>();
    MovementController = gameObject.GetComponent<MovementController>();
    NormalSpeed = MovementController.MoveSpeed;
    Player = GameManager.Instance.Player;
    CurrentAnimation = Animation.Default;
  }

  void Update()
  {
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

  private void OnTriggerEnter2D(Collider2D collision)
  {
    /*if (collision.CompareTag("PlayerSword") && Player.IsAttacking)
    {
      var health = gameObject.GetComponent<Health>();
      health.Value -= Player.CurrentDamage;
    }*/
  }


}
