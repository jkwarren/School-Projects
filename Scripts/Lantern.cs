using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    //blue
    public GameObject blueLight;
    public GameObject redLight;
    public GameObject greenLight;

    public bool colorBlue;
    public bool colorRed;
    public bool colorGreen;
    public bool colorReg;

    public bool changeColorBlue = false;
    public bool changeColorRed = false;
    public bool changeColorGreen = false;

    public bool changeColorRegular;
    public GameObject regularLight;

    public void ChangeColor()
    {
        if (changeColorBlue)
        {
            regularLight.SetActive(false);
            colorReg = false;
            redLight.SetActive(false);
            colorRed = false;
            greenLight.SetActive(false);
            colorGreen = false;
            blueLight.SetActive(true);
            colorBlue = true;
            changeColorBlue = false;
        }
        if (changeColorRegular)
        {
            blueLight.SetActive(false);
            colorBlue = false;
            redLight.SetActive(false);
            colorRed = false;
            greenLight.SetActive(false);
            colorGreen = false;
            regularLight.SetActive(true);
            colorReg = true;
            changeColorRegular = false;
        }

        if (changeColorRed)
        {
            regularLight.SetActive(false);
            colorReg = false;
            blueLight.SetActive(false);
            colorBlue = false;
            greenLight.SetActive(false);
            colorGreen = false;
            redLight.SetActive(true);
            colorRed = true;
            changeColorRed = false;
        }

        if (changeColorGreen)
        {
            regularLight.SetActive(false);
            colorReg = false;
            blueLight.SetActive(false);
            colorBlue = false;
            redLight.SetActive(false);
            colorRed = false;
            greenLight.SetActive(true);
            colorGreen = true;
            changeColorGreen = false;
        }


    }
}
