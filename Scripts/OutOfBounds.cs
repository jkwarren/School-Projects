using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public string DeathReason;
    private GameManager gameManager;
    

    private void Start()
    {
        gameManager = GameManager.Instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.KillPlayer(DeathReason); 
        }
    }

}
