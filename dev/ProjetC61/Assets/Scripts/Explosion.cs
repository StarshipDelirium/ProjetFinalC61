using UnityEngine;

public class Explosion : MonoBehaviour
{
  public enum Animation
  {
    Explosion_Radial,
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

  private void Awake()
  {
    Animator = GetComponent<Animator>();
    CurrentAnimation = Animation.Explosion_Radial;
  }

  private void OnEnable()                             // Added step for pooled objects to reset animation on respawn
  {
    Animator.Update(0);
  }

  public void ReturnToPool()                          // Triggered by animation event set to call function on last animation frame
  {
    PoolManager.Reclaim(gameObject);
  }

}
