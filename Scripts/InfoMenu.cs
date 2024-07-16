using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject infoMenuUI;
    public void ActivateInfoMenu()
    {
        infoMenuUI = GameObject.FindGameObjectWithTag("Info Menu");
        infoMenuUI.SetActive(true);
    }
    public void DeactivateInfoMenu()
    {
        infoMenuUI.SetActive(false);
    }
}
