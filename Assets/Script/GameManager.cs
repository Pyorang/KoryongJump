using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool started = false;
    private Vector3 deathLineDist;

    [SerializeField] private float maxHeight;

    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject startBooster;
    [SerializeField] private Vector3 startBoosterLocation;
    [SerializeField] private GameObject playerFollowingObjects;
    [SerializeField] private Vector3 followingObjectsLocation;
    [SerializeField] private GameObject deathLine;
    [SerializeField] private PatternSpawner patternSpawner;
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject restartMenu;
    [SerializeField] private Text heightText;
    [SerializeField] private Text heldCoinsText;
    [SerializeField] private StartLine startLine;

    private Player playerComponent;
    private Transform playerTransform;
    private static GameManager gameManager;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        playerTransform = player.GetComponent<Transform>();
        playerComponent = player.GetComponent<Player>();
        deathLineDist = playerTransform.position - deathLine.transform.position;
        StartCoroutine(CheckPlayerPosition());
    }

    private void Update()
    {
        UpdateHeightInfo();
        UpdateHeldCoins();
        CheckGameOver();
    }

    private void LateUpdate()
    {
        if (started && !playerComponent.GetIsDead())
        {
            playerFollowingObjects.transform.position = new Vector2(0, playerTransform.position.y);
        }
    }

    IEnumerator CheckPlayerPosition()
    {
        while(!started)
        {
            if (startLine.GetStarted())
                started = true;
            yield return null;
        }
    }

    public void UpdateHeightInfo()
    {
        if(!playerComponent.GetIsDead())
        {
            if (maxHeight < playerTransform.position.y)
            {
                maxHeight = playerTransform.position.y;
                heightText.text = maxHeight.ToString("F2");
                deathLine.transform.position = playerTransform.position - deathLineDist;
            }
        }
    }

    public void UpdateHeldCoins()
    {
        heldCoinsText.text = playerComponent.GetEarnCoins().ToString();
    }

    public void ResetGameManager()
    {
        started = false;
        maxHeight = 0;
        patternSpawner.ResetMaxHeight();
        patternSpawner.ResetPattern();
        pauseMenuButton.interactable = true;
        playerFollowingObjects.transform.position = followingObjectsLocation;
        Instantiate(startBooster, startBoosterLocation, Quaternion.identity);
        StartCoroutine(CheckPlayerPosition());
    }

    public void SetGameOver()
    {
        pauseMenuButton.interactable = false;
        started = false;
        //결과 처리 (재화)
        restartMenu.SetActive(true);
    }

    public void CheckGameOver()
    {
        if(playerComponent.GetIsDead())
            SetGameOver();
    }
}
