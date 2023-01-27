using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class AudioSetting : MonoBehaviour
{
    [SerializeField]private AudioMixer audioMixer;
    [SerializeField]private Slider BGMSlider;
    [SerializeField]private Slider SESlider;

    void Awake() 
    {
        float defaultVolume;

        audioMixer.GetFloat("BGM", out defaultVolume);
        BGMSlider.value = defaultVolume;

        audioMixer.GetFloat("SE", out defaultVolume);
        SESlider.value = defaultVolume;
    }

    void Update()
    {
        audioMixer.SetFloat("BGM", BGMSlider.value);
        audioMixer.SetFloat("SE", SESlider.value);
    }
    
    public void OnCloseButton()
    {
        gameObject.SetActive(false);
    }

    public void OnSliderUp(Slider slider)
    {
        slider.GetComponent<AudioSource>().Play();
    }
}
