using UnityEngine;

public class SorcererFireball : MonoBehaviour
{
  public enum Animation
  {
    FireBall,
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
  public float PoolTimer = 8;                                                       // if no impact detected, timer before object to returned to pool
  public int damage = 1;
  public float Speed = 5;
  private Vector3 direction;

  private void Awake()
  {
    Animator = GetComponent<Animator>();

  }

  private void OnEnable()
  {
    if (gameObject.transform.rotation == new Quaternion(0.0f, -90.0f, 0.0f, 0.0f))         // checks if x from rotation Quaternion is left
    {
      direction = new Vector3(-1.0f, 0.0f, 0.0f);

    }
    else
    {
      var scale = transform.localScale;                                                    // readjust and flip sprite/colliders direction
      scale.x = scale.x * -1;
      transform.localScale = scale;
      direction = new Vector3(1.0f, 0.0f, 0.0f);
    }

    Animator.Play("Fireball");
  }
  void Update()
  {
    PoolTimer -= Time.deltaTime;

    transform.position += Speed * Time.deltaTime * direction;

    if (PoolTimer <= 0)                                                             // if no impact detected after countdown, return object to parent pool 
    {
      PoolManager.Reclaim(gameObject);
      PoolTimer = 8;
    }
  }

  private void OnTriggerStay2D(Collider2D Collider)
  {
    PoolManager.Spawn(GameManager.Instance.PrefabManager.Spawn(PrefabManager.Vfx.Explosion), gameObject.transform.position, gameObject.transform.rotation);                   // On hit, trigger explosion
    GameManager.Instance.SoundManager.Play(SoundManager.Sfx.FireballExplosion);
    PoolManager.Reclaim(gameObject);          // return fireball to parent pool

  }
}
