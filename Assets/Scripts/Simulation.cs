using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public ScaleWeight leftScale;
    public ScaleWeight rightScale;

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
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("game win: " + IsGameWon());
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
}
