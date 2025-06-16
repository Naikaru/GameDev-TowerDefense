using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    

    public void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.25f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);        
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }

    public void SaveSettings()
    {
         float musicVolume = musicSlider.value;
         float sfxVolume = sfxSlider.value;

         PlayerPrefs.SetFloat("musicVolume", musicVolume);
         PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

}
