using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region

    public static SettingsManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of SettingsManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    public AudioMixer m_AudioMixer;
    private float volume;

    public void SetVolume (float volume)
    {
        m_AudioMixer.SetFloat("volume", volume);
        this.volume = volume;
    }

    public void UpdateSliderVolume(Slider slider)
    {
        slider.value = volume;
    }
}
