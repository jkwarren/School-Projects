using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBlocker : MonoBehaviour
{
    TextPanel textPanel;

    void Start()
    {
        textPanel = FindObjectOfType<TextPanel>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            textPanel.DisplayText("Hmmm, I need to find a way to fix this");
        }
    }
}
