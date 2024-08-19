using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransitionAnim : MonoBehaviour
{
    private Animator animator;
    private Image image;

    private float levelStartTime = 0f;
    private bool startAnimDone = false;

    private float START_ANIM_DURATION = 0.75f;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        levelStartTime = Time.time;
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        startAnimDone = !this.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startAnimDone && Time.time - levelStartTime > START_ANIM_DURATION)
        {
            startAnimDone = true;
            image.enabled = false;
            this.enabled = false;
        }
    }

    public void StartSandstorm()
    {
        image.enabled = true;
        animator.SetBool("stormStarting", true);
    }
}
