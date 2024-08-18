using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBGM : MonoBehaviour
{
    private static CameraBGM instance = null;
    private new AudioSource audio;

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
    }
}
