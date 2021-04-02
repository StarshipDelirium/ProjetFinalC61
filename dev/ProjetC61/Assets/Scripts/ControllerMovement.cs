using UnityEngine;

public class ControllerMovement : MonoBehaviour
{
  public float Speed;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    var horizontal = Input.GetAxisRaw("Horizontal");
    var vertical = Input.GetAxisRaw("Vertical");

    var move = new Vector3(horizontal, vertical, 0);

    if (move.magnitude > 1)
    {
      move = move.normalized;
    }


    transform.position += move * Speed * Time.deltaTime;
  }
}
