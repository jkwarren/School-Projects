using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTextEnding : MonoBehaviour
{
    private TextPanel textPanel;
    private bool alreadyToldThem = false;
    void Start()
    {
        textPanel = FindObjectOfType<TextPanel>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyToldThem)
        {
            textPanel.DisplayText("My car! I should see if it still works.");
            alreadyToldThem = true;
        }
        
    }
}
