using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioParameters : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(ProjectConst.Sfx))
        {
            PlayerPrefs.SetFloat(ProjectConst.Sfx, 0.5f);
        }
        if (PlayerPrefs.HasKey(ProjectConst.Music))
        {
            PlayerPrefs.SetFloat(ProjectConst.Music, 0.5f);
        }
        
        _sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(ProjectConst.Sfx));
        _musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(ProjectConst.Music));
        _sfxSlider.onValueChanged.AddListener(x => SetVolume(ProjectConst.Sfx, x));
        _musicSlider.onValueChanged.AddListener(x => SetVolume(ProjectConst.Music, x));
    }

    private void OnDestroy()
    {
        _sfxSlider.onValueChanged.RemoveAllListeners();
        _musicSlider.onValueChanged.RemoveAllListeners();
    }

    private void SetVolume(string volumeName, float value)
    {
        PlayerPrefs.SetFloat(volumeName, value);
        _audioMixer.SetFloat(volumeName, Mathf.Log10(value) * 20);
    }
    
    
}