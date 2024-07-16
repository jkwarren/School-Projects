using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; set; }
    Item Interact();
}

// Anything that needs to enter the player inventory
public enum Item
{
    None,          
    Axe,          
    Wood,
    Berries,
}