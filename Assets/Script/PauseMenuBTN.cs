using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuBTN : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void PauseMenuOn()
    {
        if (pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void ResumeButtonClick()
    {
        if(pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
