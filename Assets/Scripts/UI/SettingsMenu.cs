using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private int _currentRefreshRate;
    private int _currentResolutionIndex = 0;

    [Tooltip("Will toggle visibility of debug.logs")]
    [SerializeField] private bool debugMode;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (gameObject.activeSelf)
            gameObject.SetActive(false); // Disabled by default, in case left active prior to playmode
    }

    private void Start()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        
        resolutionDropdown.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRate;

        if (debugMode) Debug.Log("RefreshRate: " + _currentRefreshRate);
        
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (debugMode) Debug.Log("Resolution: " + _resolutions[i]);
            if (_resolutions[i].refreshRate == _currentRefreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + _filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _currentResolutionIndex = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = Mathf.Max(options.Count);
        resolutionDropdown.RefreshShownValue();
    }

    #region Video

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Debug.Log("Fullscreen: " + isFullScreen);
        Screen.fullScreen = isFullScreen;
    }

    #endregion

    #region Audio

    // On value change for volume slider, adjust master volume on the audioMixer
    public void SetBGMVolume(float sliderValue)
    {
        audioManager.masterBGMVolume = Mathf.Log10(sliderValue) * 20;

        if (debugMode)
            Debug.Log(sliderValue);
    }
    
    public void SetSFXVolume(float sliderValue)
    {
        audioManager.masterSFXVolume = Mathf.Log10(sliderValue) * 20;

        if (debugMode)
            Debug.Log(sliderValue);
    }

    #endregion
    
    public void CloseSettingsMenu()
    {
        gameObject.SetActive(false);
    }
}
