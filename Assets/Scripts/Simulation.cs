using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    private BlockMovement[] buildingBlocks;

    // Start is called before the first frame update
    void Start()
    {
        buildingBlocks = FindObjectsOfType<BlockMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BlockMovement[] GetAllBuildingBlocks()
    {
        return buildingBlocks;
    }
}
