using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    public AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudioAtRandomTime());
    }

    IEnumerator PlayAudioAtRandomTime()
    {
        float randomDelay = Random.Range(15f, 60f);

        yield return new WaitForSeconds(randomDelay);
        audioSource.Play();
    }
}
