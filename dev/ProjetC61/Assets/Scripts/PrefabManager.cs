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
    Possessed,
    Skeleton,
    Sorcerer,

    Count

  };

  public enum Projectiles
  {
    ElectroBall,
    SorcererFireball,               // Poolable

    Count
  }

  public enum Vfx
  {
    ElectricImpact,
    Explosion,                    // Poolable
    SaveFX,

    Count
  }

  public enum Item
  {
    CemeteryKey,

    Count

  }

  public enum Usable
  {
    HealthElixir,
    HealthPotion,
    ManaElixir,
    ManaPotion,

    Count

  }

  public GameObject[] GlobalGameObjects;
  public GameObject[] EnemyGameObjects;
  public GameObject[] ProjectilesGameObjects;
  public GameObject[] VfxGameObjects;

  public GameObject[] ItemGameObjects;
  public GameObject[] UsableGameObjects;


  private void Awake()
  {
    GlobalGameObjects = Resources.LoadAll<GameObject>("prefabs/global/character");

    EnemyGameObjects = Resources.LoadAll<GameObject>("prefabs/global/enemy");
    ProjectilesGameObjects = Resources.LoadAll<GameObject>("prefabs/global/projectiles");
    VfxGameObjects = Resources.LoadAll<GameObject>("prefabs/global/vfx");
    ItemGameObjects = Resources.LoadAll<GameObject>("prefabs/global/items");
    UsableGameObjects = Resources.LoadAll<GameObject>("prefabs/global/usables");
    Debug.Assert((int)Global.Count == GlobalGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Global.Count + ") does not match Resources folder (" + GlobalGameObjects.Length + ")");
    Debug.Assert((int)Enemy.Count == EnemyGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Enemy.Count + ") does not match Resources folder (" + EnemyGameObjects.Length + ")");
    Debug.Assert((int)Projectiles.Count == ProjectilesGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Projectiles.Count + ") does not match Resources folder (" + ProjectilesGameObjects.Length + ")");
    Debug.Assert((int)Vfx.Count == VfxGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Vfx.Count + ") does not match Resources folder (" + VfxGameObjects.Length + ")");
    Debug.Assert((int)Item.Count == ItemGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Item.Count + ") does not match Resources folder (" + ItemGameObjects.Length + ")");
    Debug.Assert((int)Usable.Count == UsableGameObjects.Length, "PrefabManager : Prefab enum length (" + (int)Usable.Count + ") does not match Resources folder (" + UsableGameObjects.Length + ")");


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

  public GameObject Spawn(Item prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = ItemGameObjects[(int)prefab];
    return Instantiate(gameObject, position, rotation);
  }
  public GameObject Spawn(Usable prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = UsableGameObjects[(int)prefab];
    return Instantiate(gameObject, position, rotation);
  }

  public GameObject Spawn(Global prefab, Vector3 vector)
  {
    GameObject gameObject = GlobalGameObjects[(int)prefab];
    return Instantiate(gameObject, vector, Quaternion.identity);
  }

  public void Spawn(Vfx prefab, Vector3 position, Quaternion rotation)
  {
    GameObject gameObject = VfxGameObjects[(int)prefab];
    Instantiate(gameObject, position, rotation);
  }


  // Prefab without coordinates to provide for Object Pooling
  public GameObject Spawn(Projectiles prefab)
  {
    GameObject gameObject = ProjectilesGameObjects[(int)prefab];
    return gameObject;
  }
  public GameObject Spawn(Vfx prefab)
  {
    GameObject gameObject = VfxGameObjects[(int)prefab];
    return gameObject;
  }
}
