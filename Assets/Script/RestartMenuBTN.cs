using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenu : MonoBehaviour
{
    [SerializeField] private Vector3 groundLocation;
    [SerializeField] private Vector3 deathLineLocation;
    [SerializeField] private StartLine startLine;
    [SerializeField] private Player player;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject restartMenu;
    [SerializeField] private GameObject deathLine;
    [SerializeField] private GameObject gameManager;

    public void RestartButtonClick()
    {
        restartMenu.SetActive(false);
        ResetPlayer();
        ResetGround();
        ResetDeathLine();
        startLine.ResetIsStarted();
        ResetGameManager();
    }

    public void ResetPlayer()
    {
        player.gameObject.SetActive(true);
        player.SetIsDead(false);
        player.ResetLife();
        player.ResetCoin();
        player.ResetLocation();
    }

    public void ResetGround()
    {
        Instantiate(groundPrefab, groundLocation, Quaternion.identity);
    }

    public void ResetDeathLine()
    {
        deathLine.transform.position = deathLineLocation;
        deathLine.GetComponent<DeathLine>().StopDestroyCoroutine();
    }

    public void ResetGameManager()
    {
        gameManager.GetComponent<GameManager>().ResetGameManager();
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }
}
