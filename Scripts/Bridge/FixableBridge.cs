using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used to disable the broken bridge object and enable the fixed bridge object

public class FixableBridge : MonoBehaviour
{
    public GameObject fixedBridge;
    public GameObject brokenBridge;
    public GameObject bridgeBlocker;
    private TextPanel textPanel;
    private GameObject player;

    void Start()
    {
        textPanel = FindObjectOfType<TextPanel>();

        fixedBridge = transform.Find("FixedBridge")?.gameObject;
        brokenBridge = transform.Find("BrokenBridge")?.gameObject;
        bridgeBlocker = transform.Find("BridgeBlocker")?.gameObject;

        player = GameObject.FindGameObjectWithTag("Player");

        if (fixedBridge != null && fixedBridge.transform.parent == transform)
        {
            fixedBridge.SetActive(false);
        }
        else
        {
            Debug.LogWarning("FixedBridge not found as a child of the Bridge GameObject.");
        }
    }

    public void FixBridge()
    {
        if (fixedBridge != null && fixedBridge.transform.parent == transform)
        {
            fixedBridge.SetActive(true);
            textPanel.DisplayText("Huh, my axe fell in the water.");
            player.GetComponent<Player>().SetAxe(false);

        }
        else
        {
            Debug.LogWarning("FixedBridge not found as a child of the Bridge GameObject.");
        }

        if (brokenBridge != null && brokenBridge.transform.parent == transform)
        {
            brokenBridge.SetActive(false);
        }
        else
        {
            Debug.LogWarning("BrokenBridge not found as a child of the Bridge GameObject.");
        }

        if (bridgeBlocker != null && bridgeBlocker.transform.parent == transform)
        {
            bridgeBlocker.SetActive(false);
        }
        else
        {
            Debug.LogWarning("BrokenBridge not found as a child of the Bridge GameObject.");
        }

    }

}
