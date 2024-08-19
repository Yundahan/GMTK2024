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

    public string volumeType;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleMute);
        mixer.GetFloat(volumeType, out float musicVolume);
        GetComponent<Image>().sprite = musicVolume != -80 ? soundOn : soundOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMute()
    {
        mixer.GetFloat(volumeType, out float musicVolume);

        if (musicVolume != -80)
        {
            mixer.SetFloat(volumeType, -80);
            GetComponent<Image>().sprite = soundOff;
        }
        else
        {
            mixer.SetFloat(volumeType, 0);
            GetComponent<Image>().sprite = soundOn;
        }
    }
}
