using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Search;

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
    public TMPro.TextMeshProUGUI UnlockCost;

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

    [SerializeField]
    public Button[] PerkSelectButton;//�� ���� ��ư(4��)

    [SerializeField]
    public TMPro.TextMeshProUGUI currentCoin;

    public DummyPlayerData PlayerData;//CurrentSelectedCharacterIndex, CharacterUnlockedList, PerkUnlockedList, CurrentCoin�� �����´�
    public DummyDataList DataList;// �ܰ� ĳ���� ������ ����Ʈ�� �����´�.

    public int currentCharacterIndex;
    public int currentPerkIndexA;
    public int currentPerkIndexB;

    public int LastSelectedPerk;
    public int LastUpdatedPerkPosition;    

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

        LastUpdatedPerkPosition = -1;
        LastSelectedPerk = -1;

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
        UnlockCost.text = currentCharacter.cost.ToString();

        //ĳ���� ���� ����
        CharacterName.text = currentCharacter.name;
        SelectedCharacterSprite = currentCharacter.image;


        //���� ��ư ���� ����
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

        //�� ���� ����




        currentCoin.text = PlayerData.currentCoin.ToString();

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
        
        
        HandleCharacterSelection();
    }

    void OnBuyButtonClicked()
    {

        var currentCharacter = DataList.allCharacter[currentCharacterIndex];

        if (PlayerData.currentCoin >= currentCharacter.cost)
        {
            PlayerData.currentCoin -= currentCharacter.cost;
            PlayerData.AvailableCharacter[currentCharacterIndex] = true;

            HandleCharacterSelection();

            Debug.Log("ĳ���� ������");

        }
        else
        {
            Debug.Log("����� ĳ���� ���� ����");
        }

        
    }


    void UpdatePerkSelectButton(int[] index) //�� �ε����� �޾Ƽ� ���� ������Ʈ�Ѵ�.
    {
        for (int i = 0; i < index.Length; i++)
        {

            PerkSelectButton[i].onClick.RemoveAllListeners();

            if (index[i] == -1)
            {
                //�� ���� ����.

                PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "None";
                PerkSelectButton[i].interactable = false;
            }
            else
            {

                if (currentPerkIndexA == index[i] || currentPerkIndexB == index[i])//�̹� ���õ� ���� ���
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkDisableButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Disable";
                }
                else if (!PlayerData.AvailablePerk[index[i]] && LastSelectedPerk != index[i])//���� ���̰� �������� ���� ���
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkChooseButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = DataList.allPerk[index[i]].Name;
                }
                else if (!PlayerData.AvailablePerk[index[i]])//���� ���̰� �ѹ� �� ���� ���
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkBuyButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = DataList.allPerk[index[i]].Cost.ToString();
                }
                else//���� �� ������ ������ �ִ� ���� ���
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkSelectButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Enable";
                }

                PerkSelectButton[i].interactable = true;
            }
        }
    }

    void OnPerkSelectButtonClicked(int index)//�̋� �� �ε����� �޴´�.
    {

    }

    void OnPerkChooseButtonClicked(int index)
    {

    }

    void OnPerkBuyButtonClicked(int index)
    {

    }

    void OnPerkDisableButtonClicked(int index)
    {

    }

    void HandleCharacterSelection()
    {
        PlayerData.currentCharacterIndex = currentCharacterIndex;

        currentPerkIndexA = -1;
        currentPerkIndexB = -1;

        LastSelectedPerk = -1;
        LastUpdatedPerkPosition = 0;

        UpdateUI();
        Debug.Log("ĳ���� ������");
    }

}
