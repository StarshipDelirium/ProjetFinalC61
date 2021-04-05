using UnityEngine;

public class Parallax : MonoBehaviour
{
  public GameObject[] BgLayers;
  private Camera mainCam;
  private Vector2 screenBounds;
  public float buffer;
  public Rigidbody2D targetPlayer;
  public float scrollingSpeed;
  public float parallaxSpeed;

  private Vector3 lastScreenPos;
  void Start()
  {
    mainCam = gameObject.GetComponent<Camera>();
    targetPlayer = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
    screenBounds = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCam.transform.position.z));
    foreach (GameObject layer in BgLayers)
    {
      LoadChildLayers(layer);
    }

    lastScreenPos = transform.position;
  }

  private void LoadChildLayers(GameObject layer)
  {
    float layerWidth = layer.GetComponent<SpriteRenderer>().bounds.size.x - buffer;
    int childrenNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / layerWidth);
    GameObject clone = Instantiate(layer) as GameObject;

    for (int i = 0; i <= childrenNeeded; ++i)
    {
      GameObject c = Instantiate(clone) as GameObject;
      c.transform.SetParent(layer.transform);
      c.transform.position = new Vector3(layerWidth * i, layer.transform.position.y, layer.transform.position.z);
      c.name = layer.name + i;
    }

    Destroy(clone);
    Destroy(layer.GetComponent<SpriteRenderer>());
  }

  private void RepositionChildLayers(GameObject layer)
  {
    Transform[] children = layer.GetComponentsInChildren<Transform>();

    if (children.Length > 1)
    {
      GameObject firstChild = children[1].gameObject;
      GameObject lastChild = children[children.Length - 1].gameObject;
      float midLayerWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x - buffer;

      if (transform.position.x + screenBounds.x > lastChild.transform.position.x + midLayerWidth)
      {
        firstChild.transform.SetAsLastSibling();
        firstChild.transform.position = new Vector3(lastChild.transform.position.x + midLayerWidth * 2, lastChild.transform.position.y, lastChild.transform.position.z);
      }
      else if (transform.position.x - screenBounds.x < firstChild.transform.position.x - midLayerWidth)
      {
        lastChild.transform.SetAsFirstSibling();
        lastChild.transform.position = new Vector3(firstChild.transform.position.x - midLayerWidth * 2, firstChild.transform.position.y, firstChild.transform.position.z);
      }
    }
  }

  private void Update()
  {
    Vector3 velocity = Vector3.zero;
    scrollingSpeed = targetPlayer.velocity.x;

    Vector3 desiredPos = transform.position + new Vector3(scrollingSpeed, 0, 0);
    Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, 0.3f);
    transform.position = smoothPos;
  }

  private void LateUpdate()
  {
    bool flipLeft = false;
    float parallaxSpeed;

    if (targetPlayer.velocity.x != 0.0f)
    {
      if (targetPlayer.velocity.x < 0.0f)
      {
        flipLeft = true;
      }

      foreach (GameObject layer in BgLayers)
      {
        RepositionChildLayers(layer);

        if (layer.name.Contains("Graveyard"))
        {
          parallaxSpeed = 0.02f;
        }
        else
        {
          parallaxSpeed = layer.name.Contains("Sky") ? 0.07f : 0.25f;
        }

        float delta = transform.position.x - lastScreenPos.x;

        if (!flipLeft)
        {
          layer.transform.Translate(Vector3.left * -Mathf.Abs(delta * parallaxSpeed) * -1);
        }
        else
        {
          layer.transform.Translate(Vector3.left * delta * parallaxSpeed);
        }
      }
    }


    lastScreenPos = transform.position;
  }
}
