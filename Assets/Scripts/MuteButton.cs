using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private Button button;
    public AudioMixer mixer;

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
        Debug.Log("§");
       mixer.GetFloat("MusicVolume", out float musicVolume);

        if (musicVolume != -80)
        {
            mixer.SetFloat("MusicVolume", -80);
            Debug.Log("F");
        }
        else
        {
            mixer.SetFloat("MusicVolume", 0);
        }

    }
}
