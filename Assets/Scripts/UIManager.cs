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
    if (GameObject.FindGameObjectWithTag("Player") != null)
    {
      GameObject characterInstance = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Character Instance: " + characterInstance.transform.position);
        characterInstance.transform.position = Vector3.zero;
        Debug.Log("Character Instance: " + characterInstance.transform.position);
      CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
      Debug.Log("Camera Follow: " + cameraFollow);
      if (cameraFollow == null)
      {
        cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();

      }
      cameraFollow.target = characterInstance.transform;

      choiceCanvas.gameObject.SetActive(false);
      HUDManager.SetActive(true);
      GenericCanvas.SetActive(true);


      Time.timeScale = 1f;

    }else{

    
    Time.timeScale = 0f;

    barbarianButton.onClick.AddListener(() => SelectCharacter(barbarianPrefab));
    sorcererButton.onClick.AddListener(() => SelectCharacter(sorcererPrefab));
    rogueButton.onClick.AddListener(() => SelectCharacter(roguePrefab));
    }
  }




  private void SelectCharacter(GameObject characterPrefab)
  {

        Vector3 spawnPosition = new Vector3(-81f, 0f, 7.7f);

        GameObject characterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

    DontDestroyOnLoad(characterInstance);
    CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
    if (cameraFollow == null)
    {
      cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
    }
    cameraFollow.target = characterInstance.transform;

    choiceCanvas.gameObject.SetActive(false);
    HUDManager.SetActive(true);
    GenericCanvas.SetActive(true);

    Time.timeScale = 1f;
  }
}


