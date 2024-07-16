using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    public Transform cam;
    public float Distance;
    public bool active = false;

    // Arraylist in case we need to add/remove interactables
    List<GameObject> interactables = new List<GameObject>();

    void Start()
    {
        GetInteractables();
        AddOutlines();
    }

    void Update()
    {
        HightlightInteractables();
    }

    void GetInteractables()
    {
        // Get all objects that implement the interactable interface
        MonoBehaviour[] interactableObjects = FindObjectsOfType<MonoBehaviour>().Where(obj => obj is IInteractable).ToArray();

        foreach (var interactable in interactableObjects)
        {
            interactables.Add(interactable.gameObject);
        }
    }

    void AddOutlines()
    {
        foreach (GameObject obj in interactables)
        {
            if (obj.GetComponent<Outline>() == null)
            {
                obj.AddComponent<Outline>();
            }
        }
    }

    void HightlightInteractables()
    {
        foreach (GameObject obj in interactables.ToArray())
        {
            if (obj == null || obj.GetComponent<IInteractable>() == null)
            {
                interactables.Remove(obj);
                continue; // Skip current iteration for null object
            }

            IInteractable interactable = obj.GetComponent<IInteractable>();
            if (interactable.CanInteract)
            {
    
                // Calculate the distance between the interactable object and the camera
                Vector3 diff = obj.transform.position - cam.position;
                float curDistance = diff.sqrMagnitude;

                // Highlight or unhighlight the object based on distance
                if (curDistance <= Distance)
                {
                    Highlight(obj);
                }
                else
                {
                    // If the object is not enabled for interaction, unhighlight it
                    Unhighlight(obj);
                }
            }
            else
            {
                // If the object can't interact, unhighlight it
                Unhighlight(obj);
            }
        }
    }


    void Highlight(GameObject closeObj)
    {
        // Enable outline for the close object
        Outline outline = closeObj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.white;
            outline.OutlineWidth = 10f;
        }
    }

    void Unhighlight(GameObject notCloseObj)
    {
        // Disable outline for the not close object
        Outline outline = notCloseObj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}
