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

  public DialogueManager DialogueManager { get; private set; }
  public InteractPrompt InteractPrompt { get; private set; }
  public SaveLoadManager SaveLoadManager { get; private set; }
  public InventoryManager InventoryManager { get; private set; }
  public Player Player { get; private set; }
  //public Level Level { get; private set; }
  //public Camera Camera { get; private set; }
  public Camera Camera { get; set; }
  public Plane[] FrustumPlanes { get; private set; }

  public bool SaveLoaded;                                           // to apply player settings easily between scenes/save/load because of singleton
  public int PlayerHP;
  public int PlayerMana;

  private bool hasGameEnded = false;

  private void Initialize()
  {
    SoundManager = GetComponentInChildren<SoundManager>();
    PrefabManager = GetComponentInChildren<PrefabManager>();
    LevelManager = GetComponentInChildren<LevelManager>();
    DialogueManager = GetComponentInChildren<DialogueManager>();
    InteractPrompt = GetComponentInChildren<InteractPrompt>();
    SaveLoadManager = GetComponentInChildren<SaveLoadManager>();
    InventoryManager = GetComponentInChildren<InventoryManager>();

    SceneManager.sceneLoaded += OnSceneLoaded;

    OnSceneLoaded();
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
  {
    OnSceneLoaded();
  }

  private void OnSceneLoaded()
  {
    Debug.Log("ON SCENE LOADED");

    Camera = FindObjectOfType<Camera>();

    //Level = FindObjectOfType<Level>();

    Player = FindObjectOfType<Player>();

    if (!SceneManager.GetActiveScene().name.Equals("MainMenu") && !SceneManager.GetActiveScene().name.Equals("Prologue") && !SceneManager.GetActiveScene().name.Equals("GameOver"))
    {
      if (!Player)
      {
        Player = FindObjectOfType<Player>();

        if (!Player)
        {
          var playerGameObject = PrefabManager.Spawn(PrefabManager.Global.Player, Vector3.zero);
          Player = playerGameObject.GetComponent<Player>();
          DontDestroyOnLoad(Player);
        }
      }
    }



    if (SaveLoaded)
    {
      LevelManager.OnSaveLoaded();
    }
    else
    {
      LevelManager.OnLevelStart();

    }
  }

  private void Update()
  {
    FrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

    /*if (Input.GetKeyDown(KeyCode.F10))
    {
      MovementController.ShowDebug = !MovementController.ShowDebug;
    }*/
  }

  public void RestartLevel()
  {
    LevelManager.GoToLevel(LevelManager.CurrentLevel, LevelManager.LevelEntranceId);
    Player.OnLevelRestart();
  }

  public bool IsInsideCamera(Renderer renderer)
  {
    if (GeometryUtility.TestPlanesAABB(FrustumPlanes, renderer.bounds))
      return true;

    return false;
  }

  public void PauseGame()
  {
    Time.timeScale = 0;
    Player.MovementController.enabled = false;
  }

  public void ResumeGame()
  {
    Time.timeScale = 1;
    Player.MovementController.enabled = true;

  }

  public void EndGame()
  {
    if(!hasGameEnded)
    {
      hasGameEnded = true;
      Debug.Log("YOU DIED");
      SoundManager.Stop();
      SceneManager.LoadScene("GameOver");
      Invoke("Restart", 9f);
    }
  }

  void Restart()
  {
    hasGameEnded = false;
    SceneManager.LoadScene("MainMenu");
  }
}
