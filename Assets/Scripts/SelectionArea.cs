using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionArea : MonoBehaviour
{
    private List<GameObject> remainingBlocks = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<BlockMovement>() != null)
        {
            remainingBlocks.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<BlockMovement>() != null)
        {
            remainingBlocks.Remove(collider.gameObject);
        }
    }

    public int GetRemainingBlocks()
    {
        return remainingBlocks.Count;
    }
}
