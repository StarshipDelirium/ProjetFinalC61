using UnityEngine;

public class FacingController : MonoBehaviour
{
  public Facing InitialFacing = Facing.W;
  public Animator Animator;

  private Facing _facing = Facing.Invalid;
  public Facing Facing
  {
    get { return _facing; }
    set
    {
      if (_facing != value)
      {
        _facing = value;

        switch (_facing)
        {
          case Facing.N:
            Animator.SetFloat("FacingY", 1.0f);
            Animator.SetFloat("FacingX", 0.0f);
            break;
          case Facing.E:
            Animator.SetFloat("FacingY", 0.0f);
            Animator.SetFloat("FacingX", 1.0f);
            break;
          case Facing.S:
            Animator.SetFloat("FacingY", -1.0f);
            Animator.SetFloat("FacingX", 0.0f);
            break;
          case Facing.W:
            Animator.SetFloat("FacingY", 0.0f);
            Animator.SetFloat("FacingX", -1.0f);
            break;
          case Facing.NE:
            Animator.SetFloat("FacingY", 1.0f);
            Animator.SetFloat("FacingX", 1.0f);
            break;
          case Facing.NW:
            Animator.SetFloat("FacingY", 1.0f);
            Animator.SetFloat("FacingX", -1.0f);
            break;
          case Facing.SE:
            Animator.SetFloat("FacingY", -1.0f);
            Animator.SetFloat("FacingX", 1.0f);
            break;
          case Facing.SW:
            Animator.SetFloat("FacingY", -1.0f);
            Animator.SetFloat("FacingX", -1.0f);
            break;
          default:
            break;

        }
      }
    }
  }
  private void Awake()
  {
    Animator = GetComponent<Animator>();
    Facing = InitialFacing;
  }
}
