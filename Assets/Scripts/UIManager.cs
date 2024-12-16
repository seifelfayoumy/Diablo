using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  [Header("UI Elements")]
  public Button barbarianButton;
  public Button sorcererButton;
  public Button rogueButton;
  public Canvas choiceCanvas;

  [Header("Prefabs")]
  public GameObject barbarianPrefab;
  public GameObject sorcererPrefab;
  public GameObject roguePrefab;
  public GameObject HUDManager;
  public GameObject GenericCanvas;

  private int characterInstance;

  [Header("Other References")]
  public Camera mainCamera;

  private void Start()
  {
    mainCamera = Camera.main;
    //check if there is player in the scene
    if (GameObject.FindGameObjectWithTag("Player") != null)
    {
      GameObject characterInstance = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Character Instance: " + characterInstance.transform.position);
        characterInstance.transform.position = Vector3.zero;
        Debug.Log("Character Instance: " + characterInstance.transform.position);
      // Attach the camera follow script
      CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
      Debug.Log("Camera Follow: " + cameraFollow);
      if (cameraFollow == null)
      {
        cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();

      }
      cameraFollow.target = characterInstance.transform;

      // Disable the choice canvas
      choiceCanvas.gameObject.SetActive(false);
      HUDManager.SetActive(true);
      GenericCanvas.SetActive(true);


      Time.timeScale = 1f;

    }else{

    
    // Freeze the game at the start
    Time.timeScale = 0f;

    // Assign button listeners
    barbarianButton.onClick.AddListener(() => SelectCharacter(barbarianPrefab));
    sorcererButton.onClick.AddListener(() => SelectCharacter(sorcererPrefab));
    rogueButton.onClick.AddListener(() => SelectCharacter(roguePrefab));
    }
  }




  private void SelectCharacter(GameObject characterPrefab)
  {
    // Spawn the selected character
    GameObject characterInstance = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);

    DontDestroyOnLoad(characterInstance);
    // Attach the camera follow script
    CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
    if (cameraFollow == null)
    {
      cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
    }
    cameraFollow.target = characterInstance.transform;

    // Disable the choice canvas
    choiceCanvas.gameObject.SetActive(false);
    HUDManager.SetActive(true);
    GenericCanvas.SetActive(true);

    // Unfreeze the game
    Time.timeScale = 1f;
  }
}


