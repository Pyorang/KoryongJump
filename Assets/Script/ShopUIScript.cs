using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUIScript : MonoBehaviour
{
    [SerializeField]
    public Button GoToMainButton;
    [SerializeField]
    public Button NextCharacterSelectButton;
    [SerializeField]
    public Button PreviousCharacterSelectButton;

    [SerializeField]
    public Image SelectedCharacterSprite;
    [SerializeField]
    public Image LockPlayerImage;

    [SerializeField]
    public TMPro.TextMeshProUGUI PerkDescreption;
    [SerializeField]
    public TMPro.TextMeshProUGUI CharacterName;

    [SerializeField]
    public Image SelectedPerkA;
    [SerializeField]
    public Image SelectedPerkB;

    [SerializeField]
    public Button BuyButton;

    public DummyPlayerData PlayerData;//CurrentSelectedCharacterIndex, CharacterUnlockedList, PerkUnlockedList, CurrentCoin�� �����´�
    public DummyDataList DataList;// �ܰ� ĳ���� ������ ����Ʈ�� �����´�.

    public int currentCharacterIndex;
    public int currentPerkIndexA;
    public int currentPerkIndexB;

    public int LastUpdatedPerk;

    public Image[] CharacterImages;
    public Image[] PerkImages;

    // Start is called before the first frame update
    void Start()
    {
        PlayerData = FindObjectOfType<DummyPlayerData>();//�÷��̾� ������ ��������
        if(PlayerData == null)
        {
            Debug.LogError("Player������ ������ �� �����ϴ�.");
        }

        DataList = FindObjectOfType<DummyDataList>();
        if(DataList == null)
        {
            Debug.LogError("�����͸� ������ �� �����ϴ�.");
        }

        currentCharacterIndex = PlayerData.currentCharacterIndex;
        currentPerkIndexA = PlayerData.currentPerkIndexA;
        currentPerkIndexB = PlayerData.currentPerkIndexB;

        LastUpdatedPerk = 0;

        GoToMainButton.onClick.AddListener(OnGoToMenuButtonClicked);
        NextCharacterSelectButton.onClick.AddListener(OnNextCharacterSelectButtonClicked);
        PreviousCharacterSelectButton.onClick.AddListener(OnPreviousCharacterSelectButtonClicked);
        BuyButton.onClick.AddListener(OnSelectButtonClicked);

        PerkDescreption.text = "This is Perk Description";


        UpdateUI();
    
    }

    void UpdateUI()
    {
        var currentCharacter = DataList.allCharacter[currentCharacterIndex];
        var currentPerkA = DataList.allPerk[currentPerkIndexA];
        var currentPerkB = DataList.allPerk[currentPerkIndexB];

        bool isCharacterUnlocked = PlayerData.AvailableCharacter[currentCharacterIndex];
        LockPlayerImage.gameObject.SetActive(!isCharacterUnlocked);

        CharacterName.text = currentCharacter.name;
        SelectedCharacterSprite = currentCharacter.image;



        TextMeshProUGUI buttonText = BuyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();


        if(currentCharacterIndex == PlayerData.currentCharacterIndex)
        {
            buttonText.text = "Selected";
            BuyButton.interactable = false;
        }
        else if (isCharacterUnlocked)
        {
            buttonText.text = "Select";

            BuyButton.onClick.RemoveAllListeners();
            BuyButton.onClick.AddListener(OnSelectButtonClicked);

            BuyButton.interactable = true;
        }
        else
        {
            buttonText.text = "Buy";

            BuyButton.onClick.RemoveAllListeners();
            BuyButton.onClick.AddListener(OnBuyButtonClicked);

            BuyButton.interactable = true;
        }

    }

    void OnGoToMenuButtonClicked()
    {
        //�� ��ȯ �Լ�
        Debug.Log("���θ޴��� ���ٰ� �����");
    }

    void OnNextCharacterSelectButtonClicked()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % DataList.allCharacter.Count;

        UpdateUI();
    }

    void OnPreviousCharacterSelectButtonClicked() {

        currentCharacterIndex = (currentCharacterIndex - 1 + DataList.allCharacter.Count) % DataList.allCharacter.Count;
        
        UpdateUI();
    }

    void OnSelectButtonClicked()
    {
        
        
        PlayerData.currentCharacterIndex = currentCharacterIndex;

        UpdateUI();
        Debug.Log("ĳ���� ������");
    }

    void OnBuyButtonClicked()
    {

        var currentCharacter = DataList.allCharacter[currentCharacterIndex];

        if (PlayerData.currentCoin >= currentCharacter.cost)
        {
            PlayerData.currentCoin -= currentCharacter.cost;
            PlayerData.currentCharacterIndex = currentCharacterIndex;
            PlayerData.AvailableCharacter[currentCharacterIndex] = true;

            Debug.Log("ĳ���� ������");

            UpdateUI();

        }
        else
        {
            Debug.Log("����� ĳ���� ���� ����");
        }

        
    }

    



    // Update is called once per frame
    void Update()
    {
        
    }
}
