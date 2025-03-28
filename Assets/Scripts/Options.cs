using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;
using System.Collections.Generic;
using UnityEngine.Localization;

public class Options : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Dropdown language;
    [SerializeField] private Button subtitles;
    [SerializeField] private Button chatFilter;
    [Header("Graphic Settings")]
    [SerializeField] private Dropdown displayMode;
    [SerializeField] private Dropdown resolution;
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

    private Resolution[] resolutions;


    void Awake()
    {
        // Retrieves a list of all available resolutions
        FetchResolutions();

        // Retrieve PlayerPref values for all General Settings
        if (PlayerPrefs.HasKey("Language")) GetLanguage(); else SetLanguage();
        if (PlayerPrefs.HasKey("Subtitles")) GetSubtitles(); else SetSubtitles();
        if (PlayerPrefs.HasKey("ChatFilter")) GetChatFilter(); else SetChatFilter();

        // Retrieve PlayerPref values for all Graphics Settings
        if (PlayerPrefs.HasKey("DisplayMode")) GetDisplayMode(); else SetDisplayMode();
        if (PlayerPrefs.HasKey("Resolution")) GetResolution(); else SetResolution();
        if (PlayerPrefs.HasKey("VSync")) GetVSync(); else SetVSync();
        if (PlayerPrefs.HasKey("GrahpicsQuality")) GetGameQuality(); else SetGameQuality();

        // Retrieve PlayerPref values for all Audio Settings
        if (PlayerPrefs.HasKey("MasterVolume")) GetMasterVolume(); else SetMasterVolume();
        if (PlayerPrefs.HasKey("MusicVolume")) GetMusicVolume(); else SetMusicVolume();
        if (PlayerPrefs.HasKey("EffectsVolume")) GetEffectsVolume(); else SetEffectsVolume();
        if (PlayerPrefs.HasKey("AmbientVolume")) GetAmbientVolume(); else SetAmbientVolume();
    }

    private void GetLanguage()
    {
        language.value = PlayerPrefs.GetInt("Language");
        SetLanguage();
    }
    
    public void SetLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[language.value];
        PlayerPrefs.SetInt("Language", language.value);

        // Refresh dropdown list for the Languages        
        UpdateLanguageDropdown();

        // Refresh dropdown list for the display mode
        UpdateDisplayModeDropdown();

        // Refresh dropdown list for the Quality Settings
        UpdateQualitySettingDropdown(); 
    }

    private void GetSubtitles()
    {
        subtitles.GetComponentInChildren<Toggle>().isOn = !bool.Parse(PlayerPrefs.GetString("SubtitlesEnable"));
        SetSubtitles();
    }

    public void SetSubtitles()
    {
        subtitles.GetComponentInChildren<Toggle>().isOn = !subtitles.GetComponentInChildren<Toggle>().isOn;
        PlayerPrefs.SetString("SubtitlesEnable", subtitles.GetComponentInChildren<Toggle>().isOn.ToString());

        if (subtitles.GetComponentInChildren<Toggle>().isOn)
        {
            subtitles.transform.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("LocaleTable", "On");
        }

        else
        {
            subtitles.transform.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("LocaleTable", "Off");
        }
    }

    private void GetChatFilter()
    {
        chatFilter.GetComponentInChildren<Toggle>().isOn = !bool.Parse(PlayerPrefs.GetString("ChatFilterEnable"));
        SetChatFilter();
    }

    public void SetChatFilter()
    {
        chatFilter.GetComponentInChildren<Toggle>().isOn = !chatFilter.GetComponentInChildren<Toggle>().isOn;
        PlayerPrefs.SetString("ChatFilterEnable", chatFilter.GetComponentInChildren<Toggle>().isOn.ToString());

        if (chatFilter.GetComponentInChildren<Toggle>().isOn)
        {
            chatFilter.transform.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("LocaleTable", "Censored");
        }

        else
        {
            chatFilter.transform.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("LocaleTable", "Uncensored");
        }
    }

    private void GetDisplayMode()
    {
        displayMode.value = PlayerPrefs.GetInt("DisplayMode");
        SetDisplayMode();
    }

    public void SetDisplayMode()
    {
        PlayerPrefs.SetInt("DisplayMode", displayMode.value);

        switch (displayMode.value)
        {
            // Fulscreen
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            // Windowed
            case 1: Screen.fullScreenMode = FullScreenMode.Windowed; break;
            // Maximised
            case 2: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
        }
    }

    private void GetResolution()
    {
        resolution.value = PlayerPrefs.GetInt("Resolution");
        SetResolution();
    }

    public void SetResolution()
    {
        PlayerPrefs.SetInt("Resolution", resolution.value);
        Resolution selectedResolution = resolutions[PlayerPrefs.GetInt("Resolution")];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }    

    private void GetVSync()
    {
        verticalSync.GetComponentInChildren<Toggle>().isOn = !bool.Parse(PlayerPrefs.GetString("VSync"));
        SetVSync();
    }

    public void SetVSync()
    {
        verticalSync.GetComponentInChildren<Toggle>().isOn = !verticalSync.GetComponentInChildren<Toggle>().isOn;
        PlayerPrefs.SetString("VSync", verticalSync.GetComponentInChildren<Toggle>().isOn.ToString());

        if (verticalSync.GetComponentInChildren<Toggle>().isOn)
        {
            verticalSync.transform.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("LocaleTable", "On");
            QualitySettings.vSyncCount = 1;
        }

        else
        {
            verticalSync.transform.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("LocaleTable", "Off");
            QualitySettings.vSyncCount = 0;
        }
    }

    private void GetGameQuality()
    {
        graphicsQuality.value = PlayerPrefs.GetInt("GrahpicsQuality");
        SetGameQuality();
    }

    public void SetGameQuality()
    {
        QualitySettings.SetQualityLevel(graphicsQuality.value);
        PlayerPrefs.SetInt("GrahpicsQuality", graphicsQuality.value);        
    }

    private void GetMasterVolume()
    {
        float volume = PlayerPrefs.GetFloat("Mastervolume");
        masterVolume.value = volume;
        masterVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }

    public void SetMasterVolume()
    {
        float volume = masterVolume.value;
        audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20);
        masterVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("Mastervolume", volume);
    }

    private void GetMusicVolume()
    {
        float volume = PlayerPrefs.GetFloat("Musicvolume");
        musicVolume.value = volume;
        musicVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }

    public void SetMusicVolume()
    {
        float volume = musicVolume.value;
        audioMixer.SetFloat("Music Volume", Mathf.Log10(volume) * 20);
        musicVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("Musicvolume", volume);
    }

    private void GetEffectsVolume()
    {
        float volume = PlayerPrefs.GetFloat("EffectsVolume");
        effectsVolume.value = volume;
        effectsVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }

    public void SetEffectsVolume()
    {
        float volume = effectsVolume.value;
        audioMixer.SetFloat("Effects Volume", Mathf.Log10(volume) * 20);
        effectsVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }

    private void GetAmbientVolume()
    {
        float volume = PlayerPrefs.GetFloat("AmbientVolume");
        ambientVolume.value = volume;
        ambientVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
    }

    public void SetAmbientVolume()
    {
        float volume = ambientVolume.value;
        audioMixer.SetFloat("Ambient Volume", Mathf.Log10(volume) * 20);
        ambientVolume.transform.GetChild(3).GetComponent<Text>().text = Mathf.Round(100 * volume).ToString();
        PlayerPrefs.SetFloat("AmbientVolume", volume);
    }

    private void UpdateLanguageDropdown()
    {
        int index = language.value;
        Locale selectedLanguage = FetchLanguage();
        List<string> languageOptions = new()
        {
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "English", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "French", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Italian", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "German", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Spanish", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Chinese (Simple)", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Japanese", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Korean", selectedLanguage)
        };

        language.ClearOptions();
        language.AddOptions(languageOptions);
        language.value = index;
    }

    private void UpdateDisplayModeDropdown()
    {
        int index = displayMode.value;
        Locale selectedLanguage = FetchLanguage();
        List<string> displayModeOptions = new()
        {
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Fullscreen", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Windowed", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Maximized", selectedLanguage)
        };

        displayMode.ClearOptions();
        displayMode.AddOptions(displayModeOptions);
        displayMode.value = index;
    }

    private void UpdateQualitySettingDropdown()
    {
        int index = graphicsQuality.value;
        Locale selectedLanguage = FetchLanguage();
        List<string> graphicOptions = new()
        {
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Low", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Medium", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "High", selectedLanguage),
            LocalizationSettings.StringDatabase.GetLocalizedString("LocaleTable", "Ultra", selectedLanguage)
        };

        graphicsQuality.ClearOptions();
        graphicsQuality.AddOptions(graphicOptions);
        graphicsQuality.value = index;
    }

    private Locale FetchLanguage()
    {
        Locale selectedLanguage = null;

        switch (language.value)
        {
            // English
            case 0: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("en"); break;
            // French
            case 1: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("fr"); break;
            // Italian
            case 2: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("it"); break;
            // German
            case 3: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("de"); break;
            // Spanish
            case 4: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("es-MX"); break;
            // Chinese (Simplified)
            case 5: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("zh-CN"); break;
            // Japanese
            case 6: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("ja-JP"); break;
            // Korean
            case 7: selectedLanguage = LocalizationSettings.AvailableLocales.GetLocale("ko-KR"); break;
        }

        return selectedLanguage;
    }

    private void FetchResolutions()
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

        resolution.ClearOptions();
        resolution.AddOptions(resolutionOptions);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();

        PlayerPrefs.SetInt("Resolution", resolution.value);
    }

    public bool SubtitlesEnabled()
    {
        return subtitles.GetComponentInChildren<Toggle>().isOn;
    }

    public bool ChatFilterEnabled()
    {
        return chatFilter.GetComponentInChildren<Toggle>().isOn;
    }
}