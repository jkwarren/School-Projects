using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<Item> inventory = new List<Item>();
    private TextPanel textPanel;
    private GameObject player;

    private void Start()
    {
        textPanel = FindObjectOfType<TextPanel>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void AddToInventory(Item item)
    {
        if (item != Item.None)
        {
            inventory.Add(item);
            HandleInventoryInteractions(item);
        }
    }

    private void HandleInventoryInteractions(Item item)
    {
        if (item == Item.Axe)
        {
            EnableChoppingTrees();
            player.GetComponent<Player>().SetAxe(true);

        }
        else if (item == Item.Wood && inventory.Count(i => i == Item.Wood) >= 2)
        {
            textPanel.DisplayText("That should be enough wood");
            EnableFixingBridge();
        }
        else if (item == Item.Berries)
        {
            if (inventory.Count(i => i == Item.Berries) == 1)
            {
                textPanel.DisplayText("I should collect more of these.");
            }

            else if (inventory.Count(i => i == Item.Berries) == 2)
            {
                textPanel.DisplayText("I should collect more of these, maybe I can use them later.");
            }

            if (inventory.Count(i => i == Item.Berries) >= 3)
            {
                textPanel.DisplayText("That should be enough berries, I wonder where I should put them.");
                EnableFillBarrel();
            }
        }
    }

    private void EnableChoppingTrees()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("ChoppableTree");

        foreach (GameObject tree in trees)
        {
            if (tree.GetComponent<IInteractable>() != null)
            {
                tree.GetComponent<IInteractable>().CanInteract = true;
            }
        }
    }

    private void EnableFixingBridge()
    {
        BrokenBridge brokenBridge = FindObjectOfType<BrokenBridge>();
        IInteractable interactableBridge = brokenBridge.GetComponent<IInteractable>();
        if (interactableBridge != null)
        {
            interactableBridge.CanInteract = true;
        }
    }

    private void EnableFillBarrel()
    {
        Barrel barrel = FindObjectOfType<Barrel>();
        IInteractable interactableBarrel = barrel.GetComponent<IInteractable>();

        if (interactableBarrel != null)
        {
            interactableBarrel.CanInteract = true;
        }
    }

    public void RemoveFromInventory(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
        }
    }

    public bool CheckInventory(Item item, int num = 1)
    {
        return inventory.Count(i => i == item) >= num;
    }

   
}
