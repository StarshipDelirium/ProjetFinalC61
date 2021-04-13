using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HitboxCreator : MonoBehaviour
{
  public Layer Layer = Layer.EnemyHitbox;

  public BoxCollider2D Collider2D { get; private set; }

  private void Awake()
  {
    var layerName = Layer.ToString();
    var newGameObject = new GameObject(layerName)
    {
      layer = LayerMask.NameToLayer(layerName)
    };

    var collider2D = GetComponent<BoxCollider2D>();
    Collider2D = newGameObject.AddComponent<BoxCollider2D>();
    Collider2D.isTrigger = true;
    Collider2D.offset = collider2D.offset;
    Collider2D.size = collider2D.size;
    Collider2D.edgeRadius = collider2D.edgeRadius;

    newGameObject.transform.parent = transform;
    newGameObject.transform.localPosition = Vector3.zero;
  }
}
