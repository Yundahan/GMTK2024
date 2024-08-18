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

    public AudioSource[] sources = new AudioSource[3];//bronze is 0, silver is 1, gold is 2
    public AudioSource[] sfx = new AudioSource[1]; //Reset is 0

    public float switchDuration = 0.5f;
    float lastSwitch;
    int currentTrack = 0;
    private int oldTrack;
    private bool switchTracks = false;

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
        foreach (AudioSource so in sources)
        {
            so.volume = 0;
            so.Play();
        }

        sources[currentTrack] = GetComponent<AudioSource>();
        sources[currentTrack].volume = 1;

        mixer.SetFloat("Volume", Mathf.Log10(slider.value) * 20);

        lastSwitch = -switchDuration;

        foreach (AudioSource s in sfx)
        {
            s.loop = false;
            s.volume = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (switchTracks)
        {
            sources[oldTrack].volume = sources[oldTrack].volume -0.05F;
            sources[currentTrack].volume = sources[currentTrack].volume + 0.05F;

            if (sources[oldTrack].volume == 0)
            {
                switchTracks = false;
            }
        }
    }

    public void PlaySFX(int sfxselection)
    {
        sfx[sfxselection].Play();
    }

    public void SetLevelTier (int level)
    {
        if (level != currentTrack)
        {
            switchTracks = true;
            oldTrack = currentTrack;
            currentTrack = level;
        }
    }
}
