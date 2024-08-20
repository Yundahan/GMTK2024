using UnityEngine;

public class ScaleArea : MonoBehaviour
{
    public Vector2[] midpoints;

    private ScaleWeight scaleweight;

    // Start is called before the first frame update
    void Start()
    {
        scaleweight = GetComponentInParent<ScaleWeight>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject go in scaleweight.GetAllHeldObjects())
        {
            Vector3[] colliderPoints = go.GetComponent<BlockMovement>().GetColliderPointsInWorldSpace();
            bool isOutside = false;

            foreach (Vector3 point in colliderPoints)
            {
                if (!IsPointInArea(point, false))
                {
                    isOutside = true;
                    break;
                }
            }

            if (!isOutside)
            {
                Vector3[] midpoints = go.GetComponent<BlockMovement>().GetMidpointsInWorldSpace();

                foreach (Vector2 midpoint in midpoints)
                {
                    if (!IsPointInArea(midpoint, true))
                    {
                        isOutside = true;
                        break;
                    }
                }
            }

            go.GetComponent<BlockMovement>().SetIsOutsideOfArea(isOutside);
        }
    }

    public bool ObjectsOutsideOfArea()
    {
        foreach (GameObject go in scaleweight.GetAllHeldObjects())
        {
            if (go.GetComponent<BlockMovement>().IsOutsideOfArea())
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPointInArea(Vector3 point, bool midpointCheck)
    {
        Vector2[] areaPoints;

        if (midpointCheck)
        {
            areaPoints = midpoints;
        } else
        {
            areaPoints = GetComponent<PolygonCollider2D>().points;
        }

        point = transform.InverseTransformPoint(point);

        foreach (Vector2 areaPoint in areaPoints)
        {
            if (Mathf.Abs(areaPoint.x - point.x) < 0.3f && Mathf.Abs(areaPoint.y - point.y) < 0.3f)
            {
                return true;
            }
        }

        return false;
    }
}
