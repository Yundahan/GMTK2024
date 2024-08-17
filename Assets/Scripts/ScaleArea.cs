using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleArea : MonoBehaviour
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

    public bool ObjectsOutsideOfArea()
    {
        foreach(GameObject go in currentCollisions)
        {
            if (!go.GetComponent<BoxCollider2D>().bounds.Contains(this.GetComponent<Collider2D>().bounds.min + new Vector3(0.01f, 0.01f, 0f)) ||
                !go.GetComponent<BoxCollider2D>().bounds.Contains(this.GetComponent<Collider2D>().bounds.max - new Vector3(0.01f, 0.01f, 0f)))
            {
                return true;
            }
        }

        return false;
    }
}
