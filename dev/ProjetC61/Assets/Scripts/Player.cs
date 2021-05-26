﻿using System.Collections;
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
    Magic
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
  public Transform LeftSpawnPoint;
  public Transform RightSpawnPoint;
  public FacingController FacingController;

  private float defaultSpeed;
  private int _currentDamage;                               // current damage dealt by player
  private IInteractable interactable;
  private ISaveable saveCheckpoint;
  public int CurrentDamage
  {
    get { return _currentDamage; }
    set { _currentDamage = value; }
  }
  public CapsuleCollider2D SwordCollider { get; private set; }
  public Health Health;
  public Mana Mana;
  public Flash Flash;
  public bool isInvincible = false;
  public float invincibleTimer = 2;
  private bool isBlocking = false;


  private void Awake()
  {
    //MovementController = GetComponent<MovementController>();
    SwordCollider = GetComponentInChildren<CapsuleCollider2D>();
    FacingController = gameObject.GetComponent<FacingController>();
    Flash = gameObject.GetComponent<Flash>();
    Health = GetComponent<Health>();
    Mana = GetComponent<Mana>();
    Health.OnHit += OnHit;
    Health.OnDeath += OnDeath;
    Mana.OnUse += OnUse;



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


    if (Input.GetButton("Fire1"))
    {
      Animator.SetTrigger("IsAttacking");
      MovementController.IsAttacking = true;
      CurrentAnimation = Animation.Attack;

      GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Attack);
    }

    if (Input.GetButton("Fire2"))
    {
      Animator.SetTrigger("IsAttacking");
      MovementController.IsAttacking = true;
      CurrentAnimation = Animation.Magic;
    }

    if (Input.GetKeyUp(KeyCode.I))
    {
      FindObjectOfType<InventoryManager>().OpenInventory();
    }
    if (Input.GetKeyUp(KeyCode.J))
    {
      GameManager.Instance.SaveLoadManager.SaveGameData(0);
    }

    if (Input.GetKeyUp(KeyCode.K))
    {
      Mana.Value -= 2;
    }

    if (Input.GetKeyUp(KeyCode.P))
    {
      FindObjectOfType<InventoryManager>().CheckInventory("HP");
    }

    if (Input.GetKeyUp(KeyCode.O))
    {
      FindObjectOfType<InventoryManager>().CheckInventory("MP");
    }

    if (Input.GetKeyUp(KeyCode.L))
    {
      GameManager.Instance.SaveLoadManager.LoadGameData();
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
      isBlocking = true;
      MovementController.Rigidbody2D.mass = 10000;
    }

    if (Input.GetKeyUp("left shift"))
    {
      isBlocking = false;
      MovementController.Rigidbody2D.mass = 1;
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

    if (Input.GetKeyUp(KeyCode.E) && interactable != null)
    {
      interactable.Interact();

    }

    if (Input.GetKeyUp(KeyCode.Tab) && interactable != null)
    {
      interactable.CancelInteraction();
    }

    if (Input.GetKeyUp(KeyCode.H) && saveCheckpoint != null)
    {
      saveCheckpoint.Save();
    }

    if (isInvincible && invincibleTimer > 0)
    {
      invincibleTimer -= Time.deltaTime;
    }
    else if (isInvincible && invincibleTimer <= 0)
    {
      isInvincible = false;
      invincibleTimer = 2;
      Flash.StopFlash();
    }
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    IInteractable interact = collision.GetComponent<IInteractable>();
    ISaveable savePoint = collision.GetComponent<ISaveable>();

    if (interact != null)                                  // verifies if collision object is an interractable object
    {
      interactable = interact;                                 // save in variable to access when player presses E     
      interactable.Prompt();

      if (savePoint != null)
      {
        saveCheckpoint = savePoint;
      }
    }
    if (!isInvincible && collision.CompareTag("Enemy"))
    {
      Transform transform = collision.GetComponentInParent<Transform>();

      int damage = collision.GetComponent<Damage>().AttackDamage;
      if (isBlocking)
      {
        damage = damage / 2;
      }
      else
      {
        StartCoroutine(Knockback(transform, collision.name));
      }

      Health.Value -= damage;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {

    if (!isInvincible && collision.CompareTag("Enemy"))
    {
      if (collision.GetComponentInParent<Damage>())
      {
        int damage = collision.GetComponentInParent<Damage>().AttackDamage;

        if (isBlocking)
        {
          damage = damage / 2;
        }
        else
        {
          Transform transform = collision.GetComponentInParent<Transform>();
          StartCoroutine(Knockback(transform, collision.name));
        }

        Health.Value -= damage;
      }
      else
      {
        Health.Value -= 1;                                                    // Sorcerer fireball pooled too quickly to get component on trigger
      }
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    IInteractable interact = collision.GetComponent<IInteractable>();
    ISaveable saveable = collision.GetComponent<ISaveable>();

    if (interact != null)
    {
      interactable.CancelInteraction();
      interactable = null;                                     // release variable for next interaction
    }

    if (saveable != null)
    {
      saveCheckpoint = null;
    }

    // reset animator trigger to hide dialogue box
  }

  private void OnChanged(Mana mana)
  {

  }

  private void OnUse(Mana mana)
  {

  }

  private void OnHit(Health health)
  {
    Flash.StartFlash();
    if (isBlocking)
    {
      CurrentAnimation = Animation.Block;
    }
    else
    {
      CurrentAnimation = Animation.Hurt;
    }

    isInvincible = true;
  }

  private void OnDeath(Health health)
  {
    gameObject.SetActive(false);
    GameManager.Instance.EndGame();
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
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.Jump);
  }

  private void OnFall(MovementController platform)
  {
    CurrentAnimation = Animation.Fall;
  }

  private void OnMoveStart(MovementController platform)
  {
    if (MovementController.IsGrounded && !isBlocking)
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
    ResumePlayerControl();
  }

  public void Cast()                                                              // to cast spell at specific frame during animation with animation event
  {
    if (FacingController.Facing == Facing.Left)
    {
      Quaternion rotation = new Quaternion(0f, -90.0f, 0.0f, 0.0f);
      PoolManager.Spawn(GameManager.Instance.PrefabManager.Spawn(PrefabManager.Projectiles.ElectroBall), LeftSpawnPoint.position, rotation);

    }
    else
    {
      Quaternion rotation = new Quaternion(0f, 90.0f, 0.0f, 0.0f);
      PoolManager.Spawn(GameManager.Instance.PrefabManager.Spawn(PrefabManager.Projectiles.ElectroBall), RightSpawnPoint.position, rotation);
    }
  }

  public void OnAttackStart()                                                   // Fired on first frame of Attack Animation
  {
    if (!MovementController.IsJumping && !MovementController.IsFalling)
    {
      MovementController.MoveSpeed = 0;                                           // Force player movement speed to 0 to avoid sliding while attacking
    }
  }
  public void OnAttackComplete()                                                // Fired on last frame of Attack Animation
  {
    MovementController.MoveSpeed = defaultSpeed;                                // Resume normal movement speed and toggle Attack off
    MovementController.IsAttacking = false;

    ResumePlayerControl();
  }

  IEnumerator Knockback(Transform collisionSource, string collisionName)
  {
    MovementController.enabled = false;
    if ("SpikesHitBox".Equals(collisionName))
    {
      MovementController.Rigidbody2D.velocity = new Vector2((transform.position.x - collisionSource.position.x) * 2, transform.position.y - MovementController.Rigidbody2D.velocity.y + 4 * 1.75f);    // knocks back player in opposite direction of collision
    }
    else
    {
      MovementController.Rigidbody2D.velocity = new Vector2((transform.position.x - collisionSource.position.x) * 2, transform.position.y - MovementController.Rigidbody2D.velocity.y * 1.75f);    // knocks back player in opposite direction of collision
    }

    yield return new WaitForSeconds(0.3f);                      // to keep player immobile in knockback position before resuming control

    MovementController.enabled = true;
    ResumePlayerControl();

  }

  private void ResumePlayerControl()
  {
    MovementController.IsAttacking = false;
    MovementController.MoveSpeed = defaultSpeed;

    if (Input.GetMouseButton(0))
    {
      CurrentAnimation = Animation.Attack;
    }
    else if (Input.GetButtonDown("Fire2"))
    {
      CurrentAnimation = Animation.Magic;
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
      else if (Input.GetKey(KeyCode.LeftShift))
      {
        CurrentAnimation = Animation.Block;
      }
      else
      {
        CurrentAnimation = Animation.Idle;
      }
    }
  }

  public void OnSaveLoaded(SaveCheckpoint saveCheckpoint)
  {

    if (saveCheckpoint != null)
    {
      transform.position = saveCheckpoint.transform.position;
    }
    else
    {
      transform.position = Vector3.zero;
    }

    MovementController.Reset();

  }
}
