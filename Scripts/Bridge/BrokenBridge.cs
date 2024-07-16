using UnityEngine;

public class BrokenBridge : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = false;

    public Item Interact()
    {
        if (transform.parent != null)
        {
            GameObject parentObject = transform.parent.gameObject;
            FixableBridge fixableBridge = parentObject.GetComponent<FixableBridge>();
            if (fixableBridge != null)
            {
                fixableBridge.FixBridge();
            }
            else
            {
                Debug.LogWarning("FixableBridge component not found on the parent GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Parent GameObject not found.");
        }

        return Item.None;
    }
}
