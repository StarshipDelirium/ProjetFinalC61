using UnityEngine;

public class PlayerSword : MonoBehaviour
{
  private int _damage;
  public int Damage
  {
    get { return _damage; }
    set { _damage = value; }
  }

  private int defaultDamage = 1;
  //private int powerDamage = 3;
  //private int specialDamage = 5;
  private bool hasHit;
  private float hitTimer;
  private Player player;
  private CapsuleCollider2D swordCollider;
  private void Awake()
  {
    player = gameObject.GetComponentInParent<Player>();
    swordCollider = gameObject.GetComponent<CapsuleCollider2D>();
    hasHit = false;
    hitTimer = 0.5f;
  }

  void Update()
  {
    if (hasHit && hitTimer > 0)
    {
      hitTimer -= Time.deltaTime;
    }
    else if (hasHit && hitTimer <= 0)
    {
      hasHit = false;
      hitTimer = 0.5f;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    var health = collision.GetComponentInParent<Health>();

    if (player.CurrentAnimation == Player.Animation.Attack && !hasHit)
    {
      Debug.Log("CAN HIT");
      hasHit = !hasHit;
      Damage = defaultDamage;
      health.Value -= Damage;

    }
  }
}
