using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using System.Collections;

public class Options : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Dropdown language;
    private bool changinglanguage = false;
    [SerializeField] private Button subtitles;
    [SerializeField] private Button chatFilter;
    [Header("Graphic Settings")]
    [SerializeField] private Dropdown displayMode;
    [SerializeField] private Dropdown resolution;
    private Resolution[] resolutions;
    [SerializeField] private Button verticalSync;
    [SerializeField] private Dropdown graphicsQuality;
    [Header("Audio Settings")]
    [SerializeField] private Dropdown outputDevice;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider effectsVolume;
    [SerializeField] private Slider ambientVolume;
    [Header("Control Settings")]
    [SerializeField] private Button orientation;
    [SerializeField] private Button forward;
    [SerializeField] private Button back;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button sprint;
    [SerializeField] private Button shoot;
    [SerializeField] private Button reload;
    [SerializeField] private Button useEquipment;
    [SerializeField] private Button throwGrenade;
    [SerializeField] private Button interact;
    [SerializeField] private Button chat;
    [SerializeField] private Button pushToTalk;
    [Header("Miscellaneous")]
    [SerializeField] private AudioClip button;
    [SerializeField][Range(0f, 1f)] private float buttonVolume;


    void Start()
    {
        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new();        
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

        resolution.AddOptions(resolutionOptions);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();

        if (PlayerPrefs.HasKey("LocaleKey"))
        {
            GetLanguage();
        }

        else
        {
            SetLanguage();
        }

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

    public void ButtonPress()
    {
        FindAnyObjectByType<AudioManager>().Play(button, buttonVolume, "effects");
    }
    
    public void SetLanguage()
    {
        if (!changinglanguage)
        {
            StartCoroutine(SetLocale(language.value));
        }
    }

    private void GetLanguage()
    {
        if (!changinglanguage)
        {
            StartCoroutine(SetLocale(PlayerPrefs.GetInt("LocaleKey")));
        }
    }

    private IEnumerator SetLocale(int localeID)
    {
        changinglanguage = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocaleKey", language.value);
        changinglanguage = false;
    }

    public void SetMasterVolume()
    {
        float volume = masterVolume.value;
        audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20);
        masterVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void GetMasterVolume()
    {
        float volume = PlayerPrefs.GetFloat("mastervolume");
        masterVolume.value = volume;
        masterVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }

    public void SetMusicVolume()
    {
        float volume = musicVolume.value;
        audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20);
        musicVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void GetMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat("musicvolume");
        musicVolume.value = volume;
        musicVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }
    public void SetEffectsVolume()
    {
        float volume = effectsVolume.value;
        audioMixer.SetFloat("Effects Volume", Mathf.Log10(volume) * 20);
        effectsVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }

    private void GetEffectsVolume()
    {
        float volume = PlayerPrefs.GetFloat("effectsvolume");
        effectsVolume.value = volume;
        effectsVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }
    public void SetAmbientVolume()
    {
        float volume = ambientVolume.value;
        audioMixer.SetFloat("Ambient Volume", Mathf.Log10(volume) * 20);
        ambientVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("ambientVolume", volume);
    }

    private void GetAmbientVolume()
    {
        float volume = PlayerPrefs.GetFloat("ambientVolume");
        ambientVolume.value = volume;
        ambientVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
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
        switch (index)
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
