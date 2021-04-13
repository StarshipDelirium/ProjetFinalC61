using UnityEngine;

/**************************************************************************************
 * Inspiré et adapté à partir du code du professeur Maël Perreault dans le cours C63  *
 *************************************************************************************/

public class PrefabManager : MonoBehaviour
{
  public enum Global
  {
    Player,

    Count
  };

  public enum Enemy
  {
    Skeleton,

    Count

  };

  /*public enum Item
  {

  }

  public enum Usable
  {

  }*/

  public GameObject[] GlobalGameObjects;
  public GameObject[] EnemyGameObjects;
  //public GameObject[] ItemGameObjects;
  //public GameObject[] UsableGameObjects;


  private void Awake()
  {
    GlobalGameObjects = Resources.LoadAll<GameObject>("prefabs/global/character");

    EnemyGameObjects = Resources.LoadAll<GameObject>("prefabs/global/enemy");
    //ItemGameObjects = Resources.LoadAll<GameObject>("prefabs/global/items");
    //UsableGameObjects = Resources.LoadAll<GameObject>("prefabs/global/usables");
    Debug.Assert((int)Global.Count == GlobalGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Global.Count + ") does not match Resources folder (" + GlobalGameObjects.Length + ")");
    Debug.Assert((int)Enemy.Count == EnemyGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Enemy.Count + ") does not match Resources folder (" + EnemyGameObjects.Length + ")");
    //Debug.Assert((int)Item.Count == ItemGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Item.Count + ") does not match Resources folder (" + ItemGameObjects.Length + ")");
    //Debug.Assert((int)Usable.Count == UsableGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Usable.Count + ") does not match Resources folder (" + UsableGameObjects.Length + ")");


  }

  // if rotation provided
  public GameObject Spawn(Global prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = GlobalGameObjects[(int)prefab];
    return Instantiate(gameObject, position, rotation);
  }

  public GameObject Spawn(Enemy prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = EnemyGameObjects[(int)prefab];
    return Instantiate(gameObject, position, rotation);
  }

  /*public GameObject Spawn(Item prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = ItemGameObjects[(int)prefab];
    return Instantiate(gameObject, position, rotation);
  }
  public GameObject Spawn(Usable prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = UsableGameObjects[(int)prefab];
    return Instantiate(gameObject, position, rotation);
  }*/

  public GameObject Spawn(Global prefab, Vector3 vector)
  {
    GameObject gameObject = GlobalGameObjects[(int)prefab];
    return Instantiate(gameObject, vector, Quaternion.identity);
  }
}
