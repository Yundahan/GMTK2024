using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWeight : MonoBehaviour
{
    private List<GameObject> currentCollisions = new List<GameObject>();

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
            currentCollisions.Add(collider.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<BlockMovement>() != null)
        {
            currentCollisions.Remove(collider.gameObject);
        }
    }

    public int GetWeight()
    {
        int weight = 0;

        foreach (GameObject go in currentCollisions) {
            if (go.GetComponent<BlockMovement>() != null 
                && !go.GetComponent<BlockMovement>().IsBeingDragged()
                && go.GetComponent<Rigidbody2D>().velocity.y == 0)
            {
                weight += Mathf.RoundToInt(go.GetComponent<Rigidbody2D>().mass);
            }
        }

        return weight;
    }
}
