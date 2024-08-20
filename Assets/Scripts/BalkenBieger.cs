using UnityEngine;

public class BalkenBieger : MonoBehaviour
{
    public ScaleWeight leftScale;
    public ScaleWeight rightScale;
    public Sprite zeigerGlowing;
    public Sprite zeigerDark;

    public GameObject zeigerParent;
    public SpriteRenderer zeiger;

    private float ANGULAR_INDICATOR_SPEED = 0.3f;

    private bool playedScaleEvenSound = true;
    private float horizontalScaleDistance;
    private float initialXScale;
    private float zeigerAngle = 0f;

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

        TurnZeiger();
    }

    private void TurnZeiger()
    {
        if (leftScale.GetWeight() < rightScale.GetWeight())
        {
            if (zeigerAngle >= -45f)
            {
                zeigerAngle -= ANGULAR_INDICATOR_SPEED;
                zeigerParent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zeigerAngle));
            }

            zeiger.sprite = zeigerDark;
            playedScaleEvenSound = false;
        } else if (leftScale.GetWeight() > rightScale.GetWeight())
        {
            if (zeigerAngle <= 45f)
            {
                zeigerAngle += ANGULAR_INDICATOR_SPEED;
                zeigerParent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zeigerAngle));
            }

            zeiger.sprite = zeigerDark;
            playedScaleEvenSound = false;
        } else //weights are even
        {
            if (Mathf.Abs(zeigerAngle) >= 0.1f)
            {
                zeigerAngle -= zeigerAngle / Mathf.Abs(zeigerAngle) * ANGULAR_INDICATOR_SPEED;
                zeigerParent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zeigerAngle));
            }

            zeiger.sprite = zeigerGlowing;

            if (!playedScaleEvenSound)
            {
                playedScaleEvenSound = true;
                FindObjectOfType<BGMHandler>().PlaySFX(2);
            }
        }
    }
}
