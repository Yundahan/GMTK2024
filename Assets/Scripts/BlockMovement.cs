using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public Vector2[] midpoints;

    private GameObject selectionArea;
    private GameObject firstScale;
    private GameObject secondScale;
    private Simulation simulation;

    private bool isBeingDragged;
    private bool isOutsideOfArea;
    private bool weightCounts;
    private Color initColor;
    private Vector3 startPosition;
    private Vector3 relativeMousePosition;
    private Vector3 home;
    private float dragTimer;

    private List<GameObject> currentCollisions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ScaleWeight[] scales = FindObjectsOfType<ScaleWeight>();
        simulation = FindObjectOfType<Simulation>();
        selectionArea = FindObjectOfType<SelectionArea>().gameObject;

        if (scales.Length >= 2)
        {
            firstScale = scales[0].gameObject;
            secondScale = scales[1].gameObject;
        }

        home = transform.position;
        initColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + relativeMousePosition;
            weightCounts = false;
        }
        
        if (!isBeingDragged && Time.time - dragTimer > 0.1f && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 0.1f)
        {
            weightCounts = true;
        }

        if(!isBeingDragged && isOutsideOfArea)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 0.6f, 0.6f, 1f);
        } else
        {
            GetComponent<SpriteRenderer>().color = initColor;
        }

        if (transform.position.y < -100f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = home;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        currentCollisions.Add(collider.gameObject);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        GameObject go = collider.gameObject;
        currentCollisions.Remove(go);

        if (GameObject.ReferenceEquals(go, selectionArea) || GameObject.ReferenceEquals(go, firstScale) || GameObject.ReferenceEquals(go, secondScale) && !isBeingDragged)
        {
            transform.position = home;
        }
    }

    void OnMouseDown()
    {
        if (simulation.IsLevelFinished())
        {
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        relativeMousePosition = transform.position - mousePosition;
        startPosition = transform.position;
        isBeingDragged = true;
    }

    void OnMouseUp()
    {
        Vector2 size = new Vector2(Mathf.Round(GetComponent<Collider2D>().bounds.size.x), Mathf.Round(GetComponent<Collider2D>().bounds.size.y));
        float x = size.x % 2 == 1 ? Mathf.Round(transform.position.x) : Mathf.Round(transform.position.x - 0.5f) + 0.5f;
        Vector3 newPosition = new Vector3(x, transform.position.y, 0f);
        Vector3 changeVector = newPosition - transform.position;
        transform.position = newPosition;
        isBeingDragged = false;
        dragTimer = Time.time;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        if (!CheckPositionValid(changeVector))
        {
            transform.position = startPosition;
        }
    }

    public bool IsBeingDragged()
    {
        return isBeingDragged;
    }

    public bool IsOutsideOfArea()
    {
        return isOutsideOfArea;
    }

    public void SetIsOutsideOfArea(bool value)
    {
        isOutsideOfArea = value;
    }

    public bool GetWeightCounts()
    {
        return weightCounts;
    }

    public void Reset()
    {
        transform.position = home;
    }

    public Vector3[] GetColliderPointsInWorldSpace()
    {
        Vector2[] colliderPoints = GetComponent<PolygonCollider2D>().points;
        Vector3[] colliderPointsInWorldSpace = colliderPoints.Select(point => transform.TransformPoint(point)).ToArray();
        return colliderPointsInWorldSpace;
    }

    private bool CheckPositionValid(Vector3 changeVector)
    {
        foreach (GameObject go in currentCollisions)
        {
            if (GameObject.ReferenceEquals(go, selectionArea) || GameObject.ReferenceEquals(go, firstScale) || GameObject.ReferenceEquals(go, secondScale))
            {
                if (go.GetComponent<BoxCollider2D>().bounds.Contains(this.GetComponent<Collider2D>().bounds.min + changeVector + new Vector3(0.01f, 0.01f, 0f)) &&
                    go.GetComponent<BoxCollider2D>().bounds.Contains(this.GetComponent<Collider2D>().bounds.max + changeVector - new Vector3(0.01f, 0.01f, 0f)))
                {
                    return true;
                } else
                {
                    return false;//return false weil this nicht komplett in der area ist wo es sein soll
                }
            }
        }

        return false;
    }
}
