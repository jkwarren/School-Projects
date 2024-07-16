using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{

    [SerializeField] private GameObject startMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        UnlockCursor();
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("IntroDriving");
    }

}
