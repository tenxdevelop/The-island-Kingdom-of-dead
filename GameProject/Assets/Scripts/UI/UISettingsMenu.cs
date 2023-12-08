using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class UISettingsMenu : MonoBehaviour
{
    private const string TAG_VOLUME = "Volume";
    private const string TAG_VOLUME_MUSIC = "MusicVolume";
    [SerializeField] private Transform m_buttonsLayer;
    [SerializeField] private AudioMixer m_mixer;
    [SerializeField] private Dropdown m_ResolutionUI;
    private Resolution[] m_resolutions;
    private int m_currentResolutionIndex = 0;
    private void Start()
    {
        m_resolutions = Screen.resolutions;
        m_ResolutionUI.ClearOptions();

        List<string> operation = new List<string>();

        for (int i = 0; i < m_resolutions.Length; i++)
        {
            string option = m_resolutions[i].width + "x" + m_resolutions[i].height;
            operation.Add(option);
            if(m_resolutions[i].width == Screen.currentResolution.width &&
               m_resolutions[i].height == Screen.currentResolution.height)
                m_currentResolutionIndex = i;
        }

        m_ResolutionUI.AddOptions(operation);
        m_ResolutionUI.value = m_currentResolutionIndex;
        m_ResolutionUI.RefreshShownValue();
    }
    public void OpenSetting()
    {
        gameObject.SetActive(true);
        m_buttonsLayer.gameObject.SetActive(false);
    }

    public void CloseSetting()
    {
        gameObject.SetActive(false);
        m_buttonsLayer.gameObject.SetActive(true);
    }

    public void SetVolume(float Volume)
    {
        m_mixer.SetFloat(TAG_VOLUME, Volume);
    }

    public void SetMusicVolume(float Volume)
    {
        m_mixer.SetFloat(TAG_VOLUME_MUSIC, Volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = m_resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
