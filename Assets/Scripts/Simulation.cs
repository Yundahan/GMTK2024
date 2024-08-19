using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Simulation : MonoBehaviour
{
    public float scaleRange;
    public string nextLevel;
    public int levelAudioTier;

    private ScaleWeight firstScale;
    private ScaleWeight secondScale;
    private BlockMovement[] buildingBlocks;
    private ScaleArea[] scaleAreas;
    private SelectionArea selectionArea;
    private LevelTransitionAnim levelTransitionAnim;

    private float LEVEL_START_TIME = 1f;
    private float LEVEL_END_TIME = 1.5f;
    private float SANDSTORM_START_TIME = 0.75f;

    private float totalBlockWeight;
    private float levelStartedTimer;
    private float levelFinishedTimer;
    private bool levelFinished;
    private bool sandstormStarted;

    static float audioVolumeValue = 0.191F;
    
    // Start is called before the first frame update
    void Start()
    {
        buildingBlocks = FindObjectsOfType<BlockMovement>();
        scaleAreas = FindObjectsOfType<ScaleArea>();
        selectionArea = FindObjectOfType<SelectionArea>();
        levelTransitionAnim = FindObjectOfType<LevelTransitionAnim>(true);
        ScaleWeight[] scales = FindObjectsOfType<ScaleWeight>();

        if (scales.Length >= 2)
        {
            firstScale = scales[0];
            secondScale = scales[1];
        }

        totalBlockWeight = 0;

        foreach (BlockMovement block in FindObjectOfType<Simulation>().GetAllBuildingBlocks())
        {
            totalBlockWeight += block.GetComponent<Rigidbody2D>().mass;
        }

        levelStartedTimer = Time.time;
        levelFinished = false;
        sandstormStarted = false;
        FindObjectOfType<BGMHandler>().SetLevelTier(levelAudioTier);
        FindObjectOfType<Slider>().value = audioVolumeValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameWon() && Time.time - levelStartedTimer > LEVEL_START_TIME && !levelFinished)
        {
            levelFinished = true;
            levelFinishedTimer = Time.time;
        }

        if (levelFinished && !sandstormStarted && Time.time - levelFinishedTimer > SANDSTORM_START_TIME)
        {
            sandstormStarted = true;
            levelTransitionAnim.enabled = true;
            levelTransitionAnim.StartSandstorm();
        }

        if (levelFinished && Time.time - levelFinishedTimer > LEVEL_END_TIME)
        {
            levelFinishedTimer = Time.time;
            NextScene();
        }
    }

    public BlockMovement[] GetAllBuildingBlocks()
    {
        return buildingBlocks;
    }

    public float GetTotalBlockWeight()
    {
        return totalBlockWeight;
    }

    public float GetScaleRange()
    {
        return scaleRange;
    }

    public bool IsLevelFinished()
    {
        return levelFinished;
    }

    public void Reset()
    {
        FindObjectOfType<BGMHandler>().PlaySFX(0);

        foreach (BlockMovement block in buildingBlocks)
        {
            block.Reset();
        }
    }

    public bool IsGameWon()
    {
        foreach (ScaleArea scaleArea in scaleAreas)
        {
            if (scaleArea.ObjectsOutsideOfArea())
            {
                return false;
            }
        }

        if (firstScale.GetWeight() != secondScale.GetWeight())
        {
            return false;
        }

        if (firstScale.GetWeight() + secondScale.GetWeight() != totalBlockWeight)
        {
            return false;
        }

        return selectionArea.GetRemainingBlocks() == 0;
    }
    public void NextScene()
    {
        audioVolumeValue = FindObjectOfType<Slider>().value;
        FindObjectOfType<BGMHandler>().PlaySFX(1);
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
