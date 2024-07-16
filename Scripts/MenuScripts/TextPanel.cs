using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPanel : MonoBehaviour
{
    [SerializeField] private GameObject textPanel;
    [SerializeField] private bool isPanel;
    public TextMeshProUGUI displayText;
    public AudioSource audioSource;

    // Static reference to the TextPanel instance
    public static TextPanel instance;

    // Awake is called before Start
    void Awake()
    {
        // Set the static reference to this instance
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.volume = 0.1F;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivatePanel();
        }
    }

    void ActivatePanel()
    {
        textPanel.SetActive(true);
    }

    void DeactivatePanel()
    {
        textPanel.SetActive(false);
    }

    public void TogglePanel()
    {
        if (textPanel.activeSelf)
        {
            DeactivatePanel();
        }
        else
        {
            ActivatePanel();
        }
    }

    public void DisplayText(string text, bool isHint=true)
    {
        // Set the display text
        displayText.text = text;

        // Only want audio for hints, otherwise its too common
        if (isHint) { audioSource?.Play(); }

        // Activate the panel
        ActivatePanel();

        // Start the timer coroutine
        StartCoroutine(TurnOffPanelAfterDelay(8f));
    }

    private IEnumerator TurnOffPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Deactivate the panel after the delay
        DeactivatePanel();
    }
}
