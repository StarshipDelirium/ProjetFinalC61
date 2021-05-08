using System.Collections.Generic;
using UnityEngine;
public static class PoolManager
{
  /********************************************************************************************
 * Taken and adapted from SimplePool.cs https://gist.github.com/quill18/5a7cfffae68892621267  *
 * Followed tutorial: https://www.raywenderlich.com/847-object-pooling-in-unity               *
 **********************************************************************************************/

  const int DEFAULT_POOL_SIZE = 5;          // small default size to avoid using to much memory, pool can grow as needed

  class ObjectPool
  {
    int nextID = 0;                         // give ID to each instantiated object for better tracking/debugging

    Stack<GameObject> inactiveObjects;      // stack and not List since no random access or search is needed
                                            // each pool will always handle a single prefab type

    GameObject prefabToPool;

    // Pool constructor
    public ObjectPool(GameObject prefab, int amountToPool)
    {
      this.prefabToPool = prefab;

      inactiveObjects = new Stack<GameObject>(amountToPool);

    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)        // sets and spawns prefab at requested coordinates
    {
      GameObject obj;

      if (inactiveObjects.Count == 0)            // When pool stack is empty, create new object
      {
        obj = (GameObject)GameObject.Instantiate(prefabToPool, position, rotation);
        obj.name = prefabToPool.name + " (" + (++nextID) + ")";

        obj.AddComponent<PoolMember>().parentPool = this;                 // assign object to a specific pool/parent so it goes back to correct pool when inactive
      }
      else
      {
        obj = inactiveObjects.Pop();                  // Get next object in line

        if (obj == null)                               // avoids exception if object no longer exists
        {
          return Spawn(position, rotation);           // recursive call to either grab next object in line or create new one
        }
      }

      obj.transform.position = position;
      obj.transform.rotation = rotation;
      obj.SetActive(true);
      return obj;

    }

    public void Reclaim(GameObject obj)             // return object to parent pool, object variables and animations must be reset upon respawn via OnEnable
    {
      obj.SetActive(false);

      inactiveObjects.Push(obj);
    }

  }

  class PoolMember : MonoBehaviour
  {
    public ObjectPool parentPool;                   // to link back to correct pool when inactive object is reclaimed
  }

  static Dictionary<GameObject, ObjectPool> activePools;

  static void Init(GameObject prefab = null, int initSize = DEFAULT_POOL_SIZE)
  {
    if (activePools == null)
    {
      activePools = new Dictionary<GameObject, ObjectPool>();

    }

    if (prefab != null && !activePools.ContainsKey(prefab))
    {
      activePools[prefab] = new ObjectPool(prefab, initSize);
    }

  }

  static public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)                          // first request for object request in-game triggers pool creation
  {
    Init(prefab);                                                 // Spawn specific prefab from corresponding pool

    return activePools[prefab].Spawn(position, rotation);
  }

  static public void Reclaim(GameObject obj)
  {
    PoolMember poolMember = obj.GetComponent<PoolMember>();

    if (poolMember == null)                                      // to avoid exception/for debugging
    {
      Debug.Log(obj.name + "is not a poolable object. Object destroyed");
      GameObject.Destroy(obj);
    }
    else
    {
      poolMember.parentPool.Reclaim(obj);                     // return to parent pool
    }
  }

}
