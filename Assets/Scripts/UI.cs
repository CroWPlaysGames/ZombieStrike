using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    private Input input;
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject HUD;
    [HideInInspector] public bool paused;
    private bool options;
    [Header("Monitor Settings")]
    [SerializeField] private Dropdown displayMode;
    [SerializeField] private Text displayModeText;
    [SerializeField] private Dropdown resolution;
    [SerializeField] private Text resolutionText;
    [SerializeField] private Dropdown vSync;
    [SerializeField] private Text vSyncText;
    private Resolution[] resolutions;
    [Header("Quality Settings")]
    [SerializeField] private Dropdown graphicsQuality;
    [SerializeField] private Text graphicsQualityText;
    [Header("Monitor Settings")]
    [SerializeField] Dropdown resolutionDropdown;
    [Header("Hardware Management")]
    [SerializeField] private Dropdown outputDevice;
    [SerializeField] private Text outputDeviceText;
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Text masterVolumeText;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Text musicVolumeText;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Text effectsVolumeText;
    [SerializeField] private Slider ambientVolumeSlider;
    [SerializeField] private Text ambientVolumeText;    



    void Start()
    {
        paused = false;
        options = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        HUD.SetActive(true);
        input = FindAnyObjectByType<Input>();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            if (resolutions[i].width.Equals(Screen.currentResolution.width) && resolutions[i].height.Equals(Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (PlayerPrefs.HasKey("masterVolume"))
        {
            GetMasterVolume();
        }

        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            GetMusicVolume();
        }

        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("effectsVolume"))
        {
            GetEffectsVolume();
        }

        else
        {
            SetEffectsVolume();
        }

        if (PlayerPrefs.HasKey("ambientVolume"))
        {
            GetAmbientVolume();
        }

        else
        {
            SetAmbientVolume();
        }
    }

    void Update()
    {
        if (input.mainMenu.WasPressedThisFrame())
        {
            if (paused && !options)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        options = false;
        HUD.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void OptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        options = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        paused = true;
        Time.timeScale = 0;
    }

    public void SetMasterVolume()
    {
        float volume = masterVolumeSlider.value;
        audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20);
        masterVolumeText.text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void GetMasterVolume()
    {
        float volume = PlayerPrefs.GetFloat("mastervolume");
        masterVolumeSlider.value = volume;
        masterVolumeText.text = Mathf.Round(100 * volume).ToString();
    }

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value;
        audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20);
        musicVolumeText.text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void GetMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat("musicvolume");
        musicVolumeSlider.value = volume;
        musicVolumeText.text = Mathf.Round(100 * volume).ToString();
    }
    public void SetEffectsVolume()
    {
        float volume = effectsVolumeSlider.value;
        audioMixer.SetFloat("Effects Volume", Mathf.Log10(volume) * 20);
        effectsVolumeText.text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }

    private void GetEffectsVolume()
    {
        float volume = PlayerPrefs.GetFloat("effectsvolume");
        effectsVolumeSlider.value = volume;
        effectsVolumeText.text = Mathf.Round(100 * volume).ToString();
    }
    public void SetAmbientVolume()
    {
        float volume = ambientVolumeSlider.value;
        audioMixer.SetFloat("Ambient Volume", Mathf.Log10(volume) * 20);
        ambientVolumeText.text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("ambientVolume", volume);
    }

    private void GetAmbientVolume()
    {
        float volume = PlayerPrefs.GetFloat("ambientVolume");
        ambientVolumeSlider.value = volume;
        ambientVolumeText.text = Mathf.Round(100 * volume).ToString();
    }

    public void SetGameQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetVSyncSetting(int index)
    {
        QualitySettings.vSyncCount = index;
    }

    public void SetWindowMode(int index)
    {
        switch(index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
