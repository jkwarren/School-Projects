using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;
    public AudioSource audioSource;
    public Renderer rend;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public Item Interact()
    {
        audioSource.Play();
        rend.enabled = false;
        Destroy(this.gameObject, 2f);

        return Item.Axe;
    }
}
