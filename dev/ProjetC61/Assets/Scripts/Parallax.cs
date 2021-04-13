using UnityEngine;
public class Parallax : MonoBehaviour
{
  public Transform[] backgrounds;
  private float[] parallaxScales;             // Proportion of camera movement to move backgrounds
  public float smoothing = 0.3f;                // To smooth parallax transition visually
  private Transform cam;
  private Vector3 previousCamPos;

  void Awake()
  {
    cam = GameManager.Instance.Camera.transform;
  }
  void Start()
  {
    previousCamPos = cam.position;
    parallaxScales = new float[backgrounds.Length];

    for (int i = 0; i < backgrounds.Length; i++)
    {
      parallaxScales[i] = backgrounds[i].position.z * -1;                                                             // scale is determined by z position, how far each layer is from the camera
    }
  }
  void FixedUpdate()
  {
    for (int i = 0; i < backgrounds.Length; i++)
    {
      float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];                                 // parallax calculated with the difference between previous and current camera position, multiplied by background's parallaxScale

      float backgroundTargetPosX = backgrounds[i].position.x + parallax;                                                              // setting a target position on X axis which is the current position plus the parallax

      Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);          // create a target position with background's current position, but with new target X position

      backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);               // assign new position to background, lerp to fade between current and target position to smooth transition
    }

    previousCamPos = cam.position;
  }
}
