using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is only for the stationary car in the woods scene that transitions into the final driving scene
public class Car : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;
    private GameManager gameManager;
    public AudioSource audioSource;

    private SceneSwitcher sceneSwitcher;


    public Item Interact()
    {
        gameManager.FadeOutScreen(gameManager.fadeDuration);
        sceneSwitcher.sceneToSwitchTo = "EndDriving";
        sceneSwitcher.SwitchSceneWithFadeOut(3);
        this.CanInteract = false;
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        return Item.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneSwitcher = this.GetComponent<SceneSwitcher>();
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

}
