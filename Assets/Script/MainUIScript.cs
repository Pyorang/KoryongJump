using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    public string GameScene = "";
    public string ShopScene = "ShopScene";

    [SerializeField]
    public GameObject SettingScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonPressed()
    {
        Debug.Log("�ΰ������� ���� ��...");

        //SceneManager.LoadScene(GameScene);
    }

    public void SettingButtonPressed()
    {
        SettingScreen.SetActive(true);
    }

    public void ShopButtonPressed()
    {
        SceneManager.LoadScene(ShopScene);

        /*
         //�ε� â �ְ� ���� �� ���
         Singleton.NextScene = ShopScene;
         SceneManager.LoadScene(LoadingScene);
         */
    }
}
