using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalkenBieger : MonoBehaviour
{
    public ScaleWeight leftScale;
    public ScaleWeight rightScale;
    public Sprite zeigerGlowing;
    public Sprite zeigerDark;

    private GameObject zeiger;

    private float horizontalScaleDistance;
    private float initialXScale;

    // Start is called before the first frame update
    void Start()
    {
        zeiger = GetComponentsInChildren<Transform>().ToList()[1].gameObject;

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

        TurnZeiger();
    }

    private void TurnZeiger()
    {
        if (leftScale.GetWeight() < rightScale.GetWeight())
        {
            zeiger.GetComponent<SpriteRenderer>().sprite = zeigerDark;
        } else if (leftScale.GetWeight() > rightScale.GetWeight())
        {
            zeiger.GetComponent<SpriteRenderer>().sprite = zeigerDark;
        } else //weights are even
        {
            zeiger.GetComponent<SpriteRenderer>().sprite = zeigerGlowing;
        }
    }
}
