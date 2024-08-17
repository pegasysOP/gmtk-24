using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string MasterVolumeKey = "Sound.MasterVolume";

    public Slider volumeSlider;

    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);   
        PlayerPrefs.SetFloat(MasterVolumeKey, volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(OnVolumeValueChanged);
    }

    public void OnStartClicked()
    {
        SceneManager.LoadScene("Main");
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
}
