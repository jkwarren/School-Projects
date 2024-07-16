using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cam;
    private float interactionRange = 7f;
    private GameManager gameManager;
    private InventoryManager inventoryManager;
    public GameObject Axe;
    private TextPanel textPanel;


    private void Start()
    {
        gameManager = GameManager.Instance;
        inventoryManager = FindObjectOfType<InventoryManager>();
        textPanel = FindObjectOfType<TextPanel>();
        textPanel.DisplayText("My car! I should find a way to get back up there.", false);

    }

    void Update()
    {
        HandleInteractions();
        HandleRestartInput();
    }

    public void SetAxe(bool active)
    {
        Axe.SetActive(active);
    }

    private void HandleInteractions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(cam.position, cam.forward, out hit, interactionRange))
            {
                TryGettingItem(hit);
            }
        }
    }

    private void TryGettingItem(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.CanInteract)
            {
                Item item = interactable.Interact();
                inventoryManager.AddToInventory(item);
                TryShowingDialogue(hit);
            }
        }
    }

    private void TryShowingDialogue(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out IDialogue dialogueComponent))
        {
            TextPanel.instance.DisplayText(dialogueComponent.dialogue, false);
        }
    }

    private void HandleRestartInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameManager.KillPlayer("Player Pressed 'R'");
        }
    }
}
