using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public AudioMixer mixer;
    public Sprite soundOn;
    public Sprite soundOff;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleMute);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMute()
    {
        mixer.GetFloat("MusicVolume", out float musicVolume);

        if (musicVolume != -80)
        {
            mixer.SetFloat("MusicVolume", -80);
            GetComponent<Image>().sprite = soundOff;
        }
        else
        {
            mixer.SetFloat("MusicVolume", 0);
            GetComponent<Image>().sprite = soundOn;
        }

    }
}
