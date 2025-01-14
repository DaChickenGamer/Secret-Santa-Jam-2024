using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private int _screenResolutionNum;
    private int _currentScreenResolutionNum;

    public Slider sfxSlider, musicSlider;
    public GameObject ingameMenu;

    private List<Vector2Int> _resolutions = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (ingameMenu != null) ingameMenu.SetActive(false);
        
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        InitializeMainMenuValues();
    }

    private void InitializeMainMenuValues()
    {
        _resolutions.Clear();

        int lastWidthAdded = 0;
        int lastHeightAdded = 0;
        
        foreach (Resolution resolution in Screen.resolutions)
        {
            Vector2Int currentResolution = new Vector2Int(resolution.width, resolution.height);

            // This current system takes out refresh rates
            if (resolution.width == lastWidthAdded && resolution.height == lastHeightAdded) continue;
            
            _resolutions.Add(currentResolution);
            
            lastWidthAdded = resolution.width;
            lastHeightAdded = resolution.height;
        }

        if (_resolutions.Count == 0)
        {
            _resolutions.Add(new Vector2Int(1920, 1080)); 
        }

        _screenResolutionNum = 0;
        
        sfxSlider.value = AudioManager.Instance.GetSfxVolume();
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
    }

    public int GetResolutionNumber()
    {
        return _screenResolutionNum;
    }

    public void SetResolutionNumber(int resolutionNumber)
    {
        _screenResolutionNum = resolutionNumber;
    }

    public Vector2Int GetCurrentResolution()
    {
        return _resolutions[GetResolutionNumber()];
    }

    public int GetResolutionCount()
    {
        return _resolutions.Count;
    }

    public void NextResolution()
    {
        SetResolutionNumber(GetResolutionNumber() + 1);

        if (GetResolutionNumber() >= _resolutions.Count)
        {
            SetResolutionNumber(0);
        }
    }

    public string GetResolutionText()
    {
        Vector2Int resolution = GetCurrentResolution();
        return $"{resolution.x}x{resolution.y}";
    }

    public void ApplyResolution()
    {
        Vector2Int resolution = GetCurrentResolution();
        _currentScreenResolutionNum = GetResolutionNumber();
        Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);
    }

    public int GetCurrentScreenResolutionNumber()
    {
        return _currentScreenResolutionNum;
    }

    public void OnSFXVolumeChanged()
    {
        AudioManager.Instance.SetSfxVolume(sfxSlider.value);
    }

    public void OnAudioVolumeChanged()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void OnIngameMenuQuitButtonClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenIngameMenu()
    {
        Time.timeScale = 0;
        ingameMenu.SetActive(true);
    }

    public void CloseIngameMenu()
    {
        ingameMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public bool GetIngameMenuActive()
    {
        return ingameMenu.activeSelf;
    }
}
