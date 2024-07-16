using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    public Light myLight;
    private float originalIntensity;// = myLight.intensity;
    private float originalRange;
    private float halfIntensity;
    private float nearlyOutIntensity;
    private float deincrementLight = .0009f;
    private float colorChangeRate = .0009f;
    public float deincrementIntensity;
    public float deincrementRange;

    public bool resetFlag = false;
    public bool halfIntensityFlag = false;
    public bool nearlyOutFlag = false;


    private Color originalColor;
    private GameManager gameManager;
    private TextPanel textPanel;
    private AudioSource audioSource;
    private Lantern lantern;

    void Start()
    {
        myLight = GetComponent<Light>();
        textPanel = FindObjectOfType<TextPanel>(); 
        gameManager = GameManager.Instance;
        originalIntensity = myLight.intensity;
        originalRange = myLight.range;
        originalColor = myLight.color;

        halfIntensity = originalIntensity / 2;
        nearlyOutIntensity = myLight.intensity / 10;

        audioSource = GetComponent<AudioSource>();
        lantern = GameObject.FindGameObjectWithTag("LanternColor").GetComponent<Lantern>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Lantern should not burn when game is paused
        if (!PauseMenu.isPaused)
        {
            BurnLantern();
            checkBurnout();
            LanternBattery(myLight.intensity);
        }

        if (resetFlag)
        {
            ResetLantern();
            resetFlag = false;
        }
    }

    public void BurnLantern()
    {
        myLight.intensity -= (deincrementLight * deincrementIntensity);
        myLight.range -= (deincrementLight * deincrementRange);
        float colorLight = myLight.color.g;
        colorLight += colorChangeRate * Time.deltaTime;
        myLight.color = new Color(myLight.color.r, colorLight, myLight.color.b, myLight.color.a);
    }

    public void ResetLantern()
    {
        lantern.changeColorRegular = true;
        lantern.ChangeColor();

        // Reset Lantern
        myLight.intensity = originalIntensity;
        myLight.range = originalRange;
        myLight.color = originalColor;

        // Reset Flags
        halfIntensityFlag = false;
        nearlyOutFlag = false;
        audioSource.Play();
    }

    void LanternBattery(float intensity)
    {
        if (!nearlyOutFlag & intensity <= nearlyOutIntensity)
        {
            textPanel.DisplayText("I'm almost out of light, I should refill at a campfire.");
            nearlyOutFlag = true;
        }
        else if (!halfIntensityFlag & intensity <= halfIntensity)
        {
            textPanel.DisplayText("Looks like my light is about halfway out, I should refill it at a campfire.");
            halfIntensityFlag = true;
        }
    }

    void checkBurnout()
    {
        if (myLight.intensity <= 0.0)
        {
            gameManager.KillPlayer("Lantern burned out\nRefill lantern at a campfire");
            ResetLantern();
        }
    }


}
