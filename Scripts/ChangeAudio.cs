using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class ChangeAudio : MonoBehaviour
{
    public AudioClip otherClip;

    IEnumerator Start()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.Play();
        yield return new WaitForSeconds(5f);
        audio.volume = 0.2F;
        audio.clip = otherClip;
        audio.Play();
    }
}
