using UnityEngine;
public class ParallaxMainMenu : MonoBehaviour
{
  /*****************************************************************************
 * From parallax tutorial: 
 * https://pressstart.vip/tutorials/2019/05/1/94/parallax-effect-in-unity.html *
 * 
 * Different parallax script will be used in-game as camera will follow played
 ******************************************************************************/
  public GameObject[] backgrounds;
  public float smoothing = 0.3f;                // To smooth parallax transition visually
  private Camera cam;
  private Vector3 previousCamPos;
  private Vector2 screenBounds;                 // To keep track of where background is positioned relative to screen bounds of current camera
  private float buffer = 0.25f;
  private float scrollSpeed = 8.0f;

  void Start()
  {
    cam = gameObject.GetComponent<Camera>();
    screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

    foreach (GameObject bgLayer in backgrounds)
    {
      loadChildObjects(bgLayer);                      // load all layers currently in component array needed for parallax effect
    }

    previousCamPos = cam.transform.position;
  }
  void loadChildObjects(GameObject bgLayer)
  {
    float layerWidth = bgLayer.GetComponentInChildren<SpriteRenderer>().bounds.size.x - buffer;               // calculate how many copies a layer needs depending on its width and screenbounds in x axis
    int childrenNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / layerWidth);                                    // Will ensure continuity of each layer without gaps as camera moves
    GameObject clone = Instantiate(bgLayer) as GameObject;                                                    // clone is created to produce child layers

    for (int i = 0; i <= childrenNeeded; i++)
    {
      GameObject layerChild = Instantiate(clone) as GameObject;
      layerChild.transform.SetParent(bgLayer.transform);
      layerChild.transform.position = new Vector3(layerWidth * i, bgLayer.transform.position.y, bgLayer.transform.position.z);          // original bgLayer is set as Parent of each child layer
      layerChild.name = bgLayer.name + i;
    }
    Destroy(clone);                                                                                                                     // SpriteRenderer component in original bgLayer is destroyed to avoid image duplication
    Destroy(bgLayer.GetComponentInChildren<SpriteRenderer>());                                                                          // will now act as a container that keeps track and repositions each child as needed                                              
  }

  void repositionChildObjects(GameObject bgLayer)
  {
    Transform[] children = bgLayer.GetComponentsInChildren<Transform>();

    if (children.Length > 1)
    {
      GameObject firstChild = children[1].gameObject;
      GameObject lastChild = children[children.Length - 1].gameObject;
      float halfLayerWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - buffer;                                       // takes the image in each layer and gives half of its width (extents)

      if (transform.position.x + screenBounds.x > lastChild.transform.position.x + halfLayerWidth)                                     // if first layer child x position + half its width is NOT on screen, first child repositioned as last child
      {
        firstChild.transform.SetAsLastSibling();
        firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfLayerWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
      }
      else if (transform.position.x - screenBounds.x < firstChild.transform.position.x - halfLayerWidth)
      {
        lastChild.transform.SetAsFirstSibling();
        lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfLayerWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
      }
    }
  }

  void Update()
  {
    Vector3 velocity = Vector3.zero;
    Vector3 desiredPosition = transform.position + new Vector3(scrollSpeed, 0, 0);                                // camera's desired position determined by current position + current scrolling speed on x axis
    Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothing);    // gradually changes current to desired position for smooth transition between child layers without jittering
    transform.position = smoothPosition;
  }
  void LateUpdate()
  {
    foreach (GameObject bgLayer in backgrounds)
    {
      repositionChildObjects(bgLayer);                                                                                  // repositioning in LateUpdate so all calculations are already done
      float parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(transform.position.z / bgLayer.transform.position.z));          // parallaxSpeed determined by layer z position, layers farthest away from camera in z position move slower
      float difference = transform.position.x - previousCamPos.x;
      bgLayer.transform.Translate(Vector3.right * difference * parallaxSpeed);
    }
    previousCamPos = transform.position;
  }
}
