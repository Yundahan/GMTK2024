using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{
    public GameObject helpMessage;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleHelp);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ToggleHelp()
    {
        helpMessage.SetActive(!helpMessage.activeSelf);
    }
}
