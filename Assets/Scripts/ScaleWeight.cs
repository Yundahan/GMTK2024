using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWeight : MonoBehaviour
{
    public SpringJoint2D otherScaleSpring;
    public Rigidbody2D scaleFloor;

    private Simulation simulation;

    private List<GameObject> currentCollisions = new List<GameObject>();
    private float initialSpringDistance;

    // Start is called before the first frame update
    void Start()
    {
        simulation = FindObjectOfType<Simulation>();
        initialSpringDistance = otherScaleSpring.distance;
    }

    // Update is called once per frame
    void Update()
    {
        float divisor = simulation.GetTotalBlockWeight() == 0 ? 1f : simulation.GetTotalBlockWeight();
        otherScaleSpring.distance = initialSpringDistance * (1f - 0.5f * GetWeight() / divisor);
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
                && Mathf.Abs(go.GetComponent<Rigidbody2D>().velocity.y - scaleFloor.velocity.y) < 0.1)
            {
                weight += Mathf.RoundToInt(go.GetComponent<Rigidbody2D>().mass);
            }
        }

        return weight;
    }
}
