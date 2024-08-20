using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private Button button;
    private Simulation simulation;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Reset);
        simulation = FindObjectOfType<Simulation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        simulation.Reset();
    }
}
