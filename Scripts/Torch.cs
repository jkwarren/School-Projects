using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;
    public bool colorBlue;
    public bool colorRed;
    public bool colorGreen;
    public bool canTurnOn;


    public GameObject turnOnLight;
    private GameObject player;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }


    void ChangeColor()
    {
        audioSource.Play();

        if (colorBlue)
        {
            player.GetComponentInChildren<Lantern>().changeColorBlue = true;
            player.GetComponentInChildren<Lantern>().ChangeColor();
        }
        if (colorRed)
        {
            player.GetComponentInChildren<Lantern>().changeColorRed = true;
            player.GetComponentInChildren<Lantern>().ChangeColor();
        }
        if (colorGreen)
        {
            player.GetComponentInChildren<Lantern>().changeColorGreen = true;
            player.GetComponentInChildren<Lantern>().ChangeColor();
        }
    }

    void TurnOn()
    {
        turnOnLight.SetActive(true);
        audioSource.Play();
    }
    public Item Interact()
    {
        if (canTurnOn)
        {
            TurnOn();
            return Item.None;
        }
        ChangeColor();
        return Item.None;
    }
}
