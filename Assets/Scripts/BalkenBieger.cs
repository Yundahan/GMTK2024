using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalkenBieger : MonoBehaviour
{
    public ScaleWeight leftScale;
    public ScaleWeight rightScale;

    private float horizontalScaleDistance;
    private float initialXScale;

    // Start is called before the first frame update
    void Start()
    {
        initialXScale = transform.localScale.x;
        horizontalScaleDistance = rightScale.transform.position.x - leftScale.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float totalScaleDistance = (rightScale.transform.position - leftScale.transform.position).magnitude;
        transform.localScale = new Vector3(initialXScale * (totalScaleDistance / horizontalScaleDistance), transform.localScale.y, transform.localScale.z);

        float angle = Mathf.Acos(horizontalScaleDistance / totalScaleDistance) * Mathf.Rad2Deg;
        bool rightScaleHigher = rightScale.transform.position.y > leftScale.transform.position.y;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rightScaleHigher ? angle : -angle));
    }
}
