using UnityEngine;

/*********************************************************************
 * Adapté du code reçu de Maël Perreault dans le cadre du cours C63
 ********************************************************************/
public class FacingController : MonoBehaviour
{
  public Animator Animator;
  public Facing InitialFacing = Facing.Left;

  private Facing _facing = Facing.Invalid;
  public Facing Facing
  {
    get { return _facing; }
    set
    {
      if (_facing != value)
      {
        _facing = value;

        if (_facing == Facing.Left)
        {
          Animator.SetFloat("FacingX", -1.0f);
        }

        else
        {
          Animator.SetFloat("FacingX", 1.0f);
        }
      }
    }
  }

  public float Direction
  {
    get { return Facing == Facing.Left ? -1 : 1; }
  }

  public void Flip()
  {
    if (Facing == Facing.Left)
      Facing = Facing.Right;
    else if (Facing == Facing.Right)
      Facing = Facing.Left;
  }
  private void Awake()
  {
    Animator = GetComponent<Animator>();
    Facing = InitialFacing;
  }
}
