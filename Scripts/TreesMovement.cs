using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = false;

    [SerializeField] private float timer_cooldown = 1f;
    private bool timer_locked_out = false;
    private GameObject player;
    public AudioSource choppingAudio;

    Vector3 treePosition;
    private float treeSpeed = 60;
    private float treeShakeAmount = 0.02f;
    public bool shakeTree;
    public int hitsToDie; // How many hits can the tree survive

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        treePosition = transform.position;

        choppingAudio = GetComponent<AudioSource>();

        // Check if AudioSource component exists
        if (choppingAudio == null)
        {
            Debug.LogError("No AudioSource component found!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shakeTree)
        {
            Vector3 tempTreePos = treePosition;
            tempTreePos.x += Mathf.Sin(Time.time * treeSpeed) * treeShakeAmount;
            transform.position = tempTreePos;

            ShakeTreeTime();
        }
        
    }

    private void ShakeTreeTime()
    {
        if (timer_locked_out == false)
        {
            timer_locked_out = true;

            DamageTree();
            StartCoroutine(ShakeTime());
        }
    }

    private void DamageTree()
    {
        hitsToDie -= 1;
        if (hitsToDie < 1)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator ShakeTime()
    {
        yield return new WaitForSeconds(timer_cooldown);
        timer_locked_out = false;
        shakeTree = false;
    }

    public Item Interact()
    {
        PlayChoppingWoodSound();
        Item item = Item.None;

        if (!shakeTree && !timer_locked_out)
        {
            // Player receives item on final hit
            if (hitsToDie == 1)
            {
                item = Item.Wood;
            }


            shakeTree = true;
            ShakeTreeTime();
        }

        return item;
    }

    private void PlayChoppingWoodSound()
    {
        if (choppingAudio != null && !choppingAudio.isPlaying)
        {
            choppingAudio.Play();
        }
    }

}
