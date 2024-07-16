using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public enum WeatherStatus
    {
        Clear,
        Cave,
        Rain,
    }

    public WeatherStatus currentWeather = WeatherStatus.Clear;

    public AudioClip clearAudio;
    public AudioClip caveAudio;
    public AudioClip rainAudio;
    private AudioSource audioSource;
    private GameObject CaveSounds;
    private bool CaveBoundry;

    public ParticleSystem rainParticleSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayAudioForWeather();

    }

    void Update()
    {
        
    }

    private void PlayAudioForWeather()
    {
        audioSource.Stop();

        switch (currentWeather)
        {
            case WeatherStatus.Clear:
                audioSource.clip = clearAudio;
                break;
            case WeatherStatus.Cave:
                audioSource.clip = caveAudio;
                break;
            case WeatherStatus.Rain:
                audioSource.clip = rainAudio;
                break;
        }
        audioSource.Play();
    }

    public void HandleClear()
    {
        currentWeather = WeatherStatus.Clear;
        StopRain();
        PlayAudioForWeather();
    }

    public void HandleCave()
    {
        currentWeather = WeatherStatus.Cave;
        StopRain();
        PlayAudioForWeather();
    }

    public void HandleRain()
    {
        currentWeather = WeatherStatus.Rain;
        StartRain();
        PlayAudioForWeather();
    }

    // We could make this stop rain as well if we want only thunder
    //void HandleThunder()
    //{
    //    currentWeather = WeatherStatus.Thunder;
    //    PlayAudioForWeather();

    //    //TODO: Implement thunder audio, thunder particles/prefab
    //}

    void StartRain()
    {
        if (rainParticleSystem != null && !rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Play();
        }
    }

    void StopRain()
    {
        if (rainParticleSystem != null && rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Stop();
            rainParticleSystem.Clear();
        }
    }

}
