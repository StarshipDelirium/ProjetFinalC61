using UnityEngine;

public class LevelBounds : MonoBehaviour
{
  public SpriteRenderer minBounds;
  public SpriteRenderer maxBounds;
  private Camera cam;
  private Transform targetPlayer;
  private bool isMin = false;
  private bool isMax = false;
  private Vector2 screenBounds;

  private void Awake()
  {
    cam = GameManager.Instance.Camera;
    targetPlayer = GameManager.Instance.Player.GetComponent<Transform>();
    screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
  }
  // Update is called once per frame
  void Update()
  {
    //Debug.Log("PLAYER X: " + targetPlayer.position.x);
    //Debug.Log("CAMERA X: " + cam.transform.position.x);

    if (minBounds.isVisible)
    {
      isMin = true;
    }
    else if (!minBounds.isVisible)
    {
      isMin = false;
    }

    if (maxBounds.isVisible)
    {

      isMax = true;
    }
    else if (!maxBounds.isVisible)
    {
      isMax = false;
    }

    if (isMin && targetPlayer.position.x > cam.transform.position.x && !isMax)
    {
      cam.GetComponent<FollowObject>().TargetTransform = targetPlayer;
    }
    else if (isMax && targetPlayer.position.x < cam.transform.position.x && !isMin)
    {
      cam.GetComponent<FollowObject>().TargetTransform = targetPlayer;
    }
    else if (isMin || isMax)
    {
      cam.GetComponent<FollowObject>().TargetTransform = cam.transform;
    }
  }
}
