using UnityEngine;
using UnityEngine.SceneManagement;


/**************************************************************************************
 * Inspiré et adapté à partir du code du professeur Maël Perreault dans le cours C63  *
 *************************************************************************************/
public class GameManager : MonoBehaviour
{
  private static GameManager _instance;
  public static GameManager Instance
  {

    get
    {
      // TODO: use a bootloader instead to create this before level is started since it can be expensive to load all assets
      if (_instance == null)
      {
        var gameManagerGameObject = Resources.Load<GameObject>("GameManager");
        var managerObject = Instantiate(gameManagerGameObject);
        _instance = managerObject.GetComponent<GameManager>();
        _instance.Initialize();

        // Prevents having to recreate the manager on scene change
        // https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
        DontDestroyOnLoad(_instance);
      }

      return _instance;
    }
  }

  public PrefabManager PrefabManager { get; private set; }
  public SoundManager SoundManager { get; private set; }
  public LevelManager LevelManager { get; private set; }
  public Huxley Huxley { get; private set; }
  //public Level Level { get; private set; }
  public Camera Camera { get; private set; }
  public Plane[] FrustumPlanes { get; private set; }

  private void Initialize()
  {
    SoundManager = GetComponentInChildren<SoundManager>();
    PrefabManager = GetComponentInChildren<PrefabManager>();
    LevelManager = GetComponentInChildren<LevelManager>();

    SceneManager.sceneLoaded += OnSceneLoaded;

    OnSceneLoaded();
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
  {
    OnSceneLoaded();
  }

  private void OnSceneLoaded()
  {
    Huxley = FindObjectOfType<Huxley>();
    //Level = FindObjectOfType<Level>();
    Camera = FindObjectOfType<Camera>();

    // Turning off a single layer by code
    //Camera.cullingMask &= ~(1 << LayerMask.NameToLayer("EnemyHitbox"));

    // Dynamically create Mario in the scene
    /*if (!Mario)

    {
      Mario = FindObjectOfType<Mario>();

      if (!Mario)
      {
        var marioGameObject = PrefabManager.Spawn(PrefabManager.PlatformerPrefab.Mario, Vector3.zero);
        Mario = marioGameObject.GetComponent<Mario>();
        DontDestroyOnLoad(Mario);
      }
    }*/

    //LevelManager.OnLevelStart();
  }

  /*private void Update()
  {
    FrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

    if (Input.GetKeyDown(KeyCode.F10))
    {
      MovementController.ShowDebug = !MovementController.ShowDebug;
    }
  }

  public void RestartLevel()
  {
    LevelManager.GoToLevel(LevelManager.CurrentLevel, LevelManager.LevelEntranceId);
    Mario.OnLevelRestart();
  }

  public bool IsInsideCamera(Renderer renderer)
  {
    if (GeometryUtility.TestPlanesAABB(FrustumPlanes, renderer.bounds))
      return true;

    return false;
  }*/
}
