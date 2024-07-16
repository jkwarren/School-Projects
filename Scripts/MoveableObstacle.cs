using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObstacle : MonoBehaviour
{

    public SpecialTorch blueTorch;
    public SpecialTorch redTorch;
    public SpecialTorch greenTorch;
    public AudioSource audioSource;

    public bool audioPlaying = false;
    public float moveSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckColors())
        {
            MoveAway();
        }
    }

    private bool CheckColors()
    {
        return blueTorch.isCorrectColor && redTorch.isCorrectColor && greenTorch.isCorrectColor;
    }

    private void MoveAway()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (!audioPlaying)
        {
            audioSource.Play();
            audioPlaying = true;
        }
        
        Destroy(gameObject, 8f);
    }



}
