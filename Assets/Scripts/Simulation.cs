using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Simulation : MonoBehaviour
{
    public ScaleWeight leftScale;
    public ScaleWeight rightScale;

    public float scaleRange;
    public string nextLevel;

    private BlockMovement[] buildingBlocks;
    private ScaleArea[] scaleAreas;
    private SelectionArea selectionArea;

    private float totalBlockWeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        buildingBlocks = FindObjectsOfType<BlockMovement>();
        scaleAreas = FindObjectsOfType<ScaleArea>();
        selectionArea = FindObjectOfType<SelectionArea>();

        foreach (BlockMovement block in FindObjectOfType<Simulation>().GetAllBuildingBlocks())
        {
            totalBlockWeight += block.GetComponent<Rigidbody2D>().mass;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameWon())
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

    public bool IsGameWon()
    {
        foreach (ScaleArea scaleArea in scaleAreas)
        {
            if (scaleArea.ObjectsOutsideOfArea())
            {
                return false;
            }
        }

        if (leftScale.GetWeight() != rightScale.GetWeight())
        {
            return false;
        }

        return selectionArea.GetRemainingBlocks() == 0;
    }
    public void NextScene()
    {
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
