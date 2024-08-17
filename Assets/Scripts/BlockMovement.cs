using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public GameObject selectionArea;
    public GameObject leftScale;
    public GameObject rightScale;

    private bool isBeingDragged;
    private Vector3 startPosition;
    private Vector3 relativeMousePosition;

    private List<GameObject> currentCollisions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + relativeMousePosition;
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

        if (GameObject.ReferenceEquals(go, selectionArea) || GameObject.ReferenceEquals(go, leftScale) || GameObject.ReferenceEquals(go, rightScale) && !isBeingDragged)
        {
            transform.position = selectionArea.transform.position;
        }
    }

    void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        relativeMousePosition = transform.position - mousePosition;
        startPosition = transform.position;
        isBeingDragged = true;
    }

    void OnMouseUp()
    {
        Vector2 size = new Vector2(Mathf.Round(GetComponent<Collider2D>().bounds.size.x), Mathf.Round(GetComponent<Collider2D>().bounds.size.y));
        float x = size.x % 2 == 1 ? Mathf.Round(transform.position.x) : Mathf.Round(transform.position.x - 0.5f) + 0.5f;
        float y = size.y % 2 == 1 ? Mathf.Round(transform.position.y) : Mathf.Round(transform.position.y - 0.5f) + 0.5f;
        Vector3 newPosition = new Vector3(x, y, 0f);
        Vector3 changeVector = newPosition - transform.position;
        transform.position = newPosition;
        isBeingDragged = false;
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

    private bool CheckPositionValid(Vector3 changeVector)
    {
        bool valid = false;

        foreach (GameObject go in currentCollisions)
        {
            if (GameObject.ReferenceEquals(go, selectionArea) || GameObject.ReferenceEquals(go, leftScale) || GameObject.ReferenceEquals(go, rightScale))
            {
                if (go.GetComponent<BoxCollider2D>().bounds.Contains(this.GetComponent<Collider2D>().bounds.min + changeVector + new Vector3(0.01f, 0.01f, 0f)) &&
                    go.GetComponent<BoxCollider2D>().bounds.Contains(this.GetComponent<Collider2D>().bounds.max + changeVector - new Vector3(0.01f, 0.01f, 0f)))
                {
                    valid = true;//kein return weil noch ein anderer block an der stelle sein könnte was die position invalide macht
                } else
                {
                    return false;//return weil this nicht komplett in der area ist wo es sein soll
                }
            }
        }

        return valid;
    }
}
