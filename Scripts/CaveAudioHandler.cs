using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveAudioHandler : MonoBehaviour
{
    public bool inCave ;
    private WeatherManager weatherManager;

    // Start is called before the first frame update
    void Start()
    {
        weatherManager = FindObjectOfType<WeatherManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        inCave = !inCave;
        if (inCave)
        {
            weatherManager.HandleCave();
        } else
        {
            weatherManager.HandleClear();
        }
    }
}
