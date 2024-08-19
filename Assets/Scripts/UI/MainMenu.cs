using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string MasterVolumeKey = "Sound.MasterVolume";

    public Slider volumeSlider;

    public GameObject menuContainer;
    public TextMeshProUGUI startButtonText;
    protected bool isInTitleMenu;

    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);   
        PlayerPrefs.SetFloat(MasterVolumeKey, volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(OnVolumeValueChanged);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (IsInGameCam())
            {
                // checks if menu container game object is open
                SetMenu(menuContainer.activeInHierarchy);
            }
            else if (IsInFlyThroughCam())
            {
                Debug.Log("is in fly cam");
                GameManager gameManager = GameManager.Instance;
                if (gameManager == null)
                {
                    Debug.Log("Game manager is null");
                    return;
                }

                CameraMan cameraMan = gameManager.cameraMan;
                if (cameraMan == null)
                {
                    Debug.Log("Camera man is null");
                }
                cameraMan.ForceGameCam();
            }
            else
            {
                Debug.Log("Ignoring ESC as not in game camera");
            }
        }
    }

    private void SetMenu(bool isMenuOpen)
    {
        if (isMenuOpen)
        {
            // close menu
            ResumeGame();
        }
        else
        {
            DisplayMenu();
        }
    }

    private void DisplayMenu()
    {
        // pause timescale
        Time.timeScale = 0f;

        // show buttons and menu UI
        menuContainer.SetActive(true);

        isInTitleMenu = true;
    }

    private void ResumeGame()
    {
        // reset timescale to normal
        Time.timeScale = 1f;

        // hide pause UI
        menuContainer.SetActive(false);

        isInTitleMenu = false;
    }

    public void OnStartClicked()
    {
        // hack to not break other things 
        if (SceneManager.GetActiveScene().name == "harry-test chew")
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                Debug.Log("Game manager is null");
                return;
            }

            CameraMan cameraMan = gameManager.cameraMan;
            if (cameraMan == null)
            {
                Debug.Log("Camera man is null");
            }

            if (cameraMan.IsVirtualCameraActive(cameraMan.mainMenuVirtualCamera))
            {
                Debug.Log("Start clicked menu cam -> fly cam");
                cameraMan.TransitionFromMenuToIntro();
            }
            else if (cameraMan.IsVirtualCameraActive(cameraMan.gameVirtualCamera) || cameraMan.IsVirtualCameraActive(cameraMan.noInputGameVirtualCamera))
            {
                ResumeGame();
            }
            else
            {
                Debug.Log("No handling for start click");
            }
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
        Debug.Log("START");
    }

    public void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }

    protected virtual void OnVolumeValueChanged(float newValue)
    {
        SaveNewVolume(newValue);
    }

    protected void SaveNewVolume(float newValue)
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, newValue);
        PlayerPrefs.Save();
    }


    private bool IsInGameCam()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.Log("Game manager is null");
            return false;
        }

        CameraMan cameraMan = gameManager.cameraMan;
        if (cameraMan == null)
        {
            Debug.Log("Camera man is null");
            return false;
        }

        if (cameraMan.IsVirtualCameraActive(cameraMan.gameVirtualCamera) || cameraMan.IsVirtualCameraActive(cameraMan.noInputGameVirtualCamera))
        {
            return true;
        }

        return false;
    }

    private bool IsInFlyThroughCam()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.Log("Game manager is null");
            return false;
        }

        CameraMan cameraMan = gameManager.cameraMan;
        if (cameraMan == null)
        {
            Debug.Log("Camera man is null");
            return false;
        }

        if (cameraMan.IsVirtualCameraActive(cameraMan.flyThroughVirtualCamera))
        {
            return true;
        }

        return false;
    }
}
