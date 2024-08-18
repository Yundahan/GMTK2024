using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    private float LEVEL_START_TIME = 1f;
    private float LEVEL_END_TIME = 1f;

    private float totalBlockWeight;
    private float levelStartedTimer;
    private float levelFinishedTimer;
    private bool levelFinished;

    static float audioVolumeValue = 0.191F;


    
    // Start is called before the first frame update
    void Start()
    {
        buildingBlocks = FindObjectsOfType<BlockMovement>();
        scaleAreas = FindObjectsOfType<ScaleArea>();
        selectionArea = FindObjectOfType<SelectionArea>();
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

        if (levelFinished && Time.time - levelFinishedTimer > LEVEL_END_TIME)
        {
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

    public void Reset()
    {
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

        return selectionArea.GetRemainingBlocks() == 0;
    }
    public void NextScene()
    {
        audioVolumeValue = FindObjectOfType<Slider>().value;
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
