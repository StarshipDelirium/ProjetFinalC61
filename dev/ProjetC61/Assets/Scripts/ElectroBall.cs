using UnityEngine;

public class ElectroBall : MonoBehaviour
{
  public enum Animation
  {
    Cast,
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
  public BoxCollider2D projectileCollider;
  public Animator Animator;
  public float Speed = 7;
  public float PoolTimer = 8;
  private Vector3 direction;
  private int damage = 2;

  private void Awake()
  {
    Animator = GetComponent<Animator>();

  }

  private void Update()
  {

    transform.position += Speed * Time.deltaTime * direction;

    PoolTimer -= Time.deltaTime;

    //transform.position += Speed * Time.deltaTime * direction;

    if (PoolTimer <= 0)                                                             // if no impact detected after countdown, return object to parent pool 
    {
      PoolManager.Reclaim(gameObject);
      PoolTimer = 8;
    }

  }
  private void OnTriggerStay2D(Collider2D collision)
  {
    Health health = collision.GetComponentInParent<Health>();
    health.Value -= damage;
    PoolManager.Spawn(GameManager.Instance.PrefabManager.Spawn(PrefabManager.Vfx.ElectricImpact), gameObject.transform.position, gameObject.transform.rotation);
    PoolManager.Reclaim(gameObject);          // return fireball to parent pool


  }

  private void OnEnable()                             // Added step for pooled objects to reset animation on respawn
  {
    Animator.Update(0);
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

    CurrentAnimation = Animation.Cast;
  }

  public void ReturnToPool()                          // Triggered by animation event set to call function on last animation frame
  {
    PoolManager.Reclaim(gameObject);
  }
}
