using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBush : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;
    public GameObject berryParent;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public Item Interact()
    {
        this.CanInteract = false;
        berryParent.SetActive(false);
        audioSource.Play();

        return Item.Berries;
    }

}
