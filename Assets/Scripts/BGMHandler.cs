using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMHandler : MonoBehaviour
{
    private static BGMHandler instance = null;
    //public new AudioSource audio;
    public AudioMixer mixer;

    public Slider slider;

    static float audioTimer;
    static float audioVolumeValue = 0.1F;
    public AudioSource[] sources = new AudioSource[3];//bronze is 0, silver is 1, gold is 2
   //public AudioSource[] sfx = new AudioSource[8]; For SFX

    public float switchDuration = 0.5f;
    float lastSwitch;
    int current = 0;




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
        sources[current] = GetComponent<AudioSource>();
        sources[current].Play();

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





    void Start()
    {
        lastSwitch = -switchDuration;

        foreach (AudioSource s in sfx)
        {
            s.loop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceSwitch = Time.time - lastSwitch;

        if (timeSinceSwitch <= switchDuration)
        {
            sources[current].volume = timeSinceSwitch;
            sources[1 - current].volume = switchDuration - timeSinceSwitch;
        }
    }

    public void SwitchTracks()
    {
        current = 1 - current;
        lastSwitch = Time.time;
    }

    public void Reset(int value)
    {
        if (value != current)
        {
            SwitchTracks();
        }
    }
}
