using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CameraBGM : MonoBehaviour
{
    private static CameraBGM instance = null;
    public new AudioSource audio;
    public AudioMixer mixer;

    public Slider slider;

    static float audioTimer;
    static float audioVolumeValue = 0.1F;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (instance == this) return;
        Destroy(gameObject);


        
 
     

    }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();

        mixer.SetFloat("Volume", Mathf.Log10(slider.value) * 20);
    }

    public float GetAudioTimer()
    {
        return audioTimer;
    }

    public float GetAudioVolumeValue()
    {
        return audioVolumeValue;
    }
}
