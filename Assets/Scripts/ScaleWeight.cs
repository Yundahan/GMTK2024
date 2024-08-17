using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWeight : MonoBehaviour
{
    public Rigidbody2D scaleFloor;
    public ScaleWeight otherScale;
    public float SPEED = 1f;

    private Simulation simulation;
    private List<GameObject> currentCollisions = new List<GameObject>();

    private float restingY;

    // Start is called before the first frame update
    void Start()
    {
        simulation = FindObjectOfType<Simulation>();
        restingY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleRange = simulation.GetScaleRange();
        float totalWeight = simulation.GetTotalBlockWeight();
        float weight = GetWeight();
        float otherScaleWeight = otherScale.GetWeight();

        float goalY = restingY + scaleRange * ((otherScaleWeight - weight) / totalWeight);
        
        if (Mathf.Abs(goalY - transform.position.y) > 0.01f)
        {
            Vector3 direction = new Vector3(transform.position.x, goalY, 0f) - transform.position;
            transform.position += direction.normalized * Time.deltaTime * SPEED;
        }
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
            if (go.GetComponent<BlockMovement>() != null && !go.GetComponent<BlockMovement>().IsBeingDragged())
            {
                weight += Mathf.RoundToInt(go.GetComponent<Rigidbody2D>().mass);
            }
        }

        return weight;
    }
}
