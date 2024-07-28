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
    public Image LockPerkImage;

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
        var currentPerkA = currentPerkIndexA==-1?null:DataList.allPerk[currentPerkIndexA];
        var currentPerkB = currentPerkIndexB==-1?null:DataList.allPerk[currentPerkIndexB];

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

        LockPerkImage.gameObject.SetActive(currentCharacterIndex != PlayerData.currentCharacterIndex);

        UpdatePerkSelectButton(currentCharacter.perks.ToArray());
        if(LastSelectedPerk == -1)
        {
            PerkDescreption.text = "This is Perk Description";
        }
        else
        {
            PerkDescreption.text = DataList.allPerk[LastSelectedPerk].Description;
        }

        /* �� �̹��� ���� �ڵ� */


        currentCoin.text = PlayerData.currentCoin.ToString();

    }

    void OnGoToMenuButtonClicked()
    {
        SyncPlayerData();
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


    void UpdatePerkSelectButton(int[] index)
    {
        for (int i = 0; i < index.Length; i++)
        {
            //Debug.Log("CurrentIndex: " + index[i]);
            var button = PerkSelectButton[i];
            var text = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            button.onClick.RemoveAllListeners();

            int TempIndex = index[i];//AddListener ���� �����Լ��� �Ű����� ���� ���� �ذ��� ���� �ӽ� ����

            if (index[i] == -1)// �������� �ʴ� ���
            {
                text.text = "None";
                button.interactable = false;
            }
            else
            {
                bool isSelected = currentPerkIndexA == index[i] || currentPerkIndexB == index[i];
                bool isAvailable = PlayerData.AvailablePerk[index[i]];
                bool isLastSelected = LastSelectedPerk == index[i];

                if (isSelected)// �̹� ���õ� ���� ���
                {
                    text.text = "Disable";
                    button.onClick.AddListener(() => OnPerkDisableButtonClicked(TempIndex));
                }
                else if (!isAvailable && !isLastSelected) // ���õ��� �ʾҰ�, �̿� �Ұ����� ���
                {
                    text.text = DataList.allPerk[index[i]].Name;
                    button.onClick.AddListener(() => OnPerkChooseButtonClicked(TempIndex));
                }
                else if (!isAvailable) // ���õ��� ���� ���
                {
                    text.text = DataList.allPerk[index[i]].Cost.ToString();
                    button.onClick.AddListener(() => OnPerkBuyButtonClicked(TempIndex));
                }
                else //������ �� �ִ� ���
                {
                    text.text = "Enable";
                    button.onClick.AddListener(() => OnPerkSelectButtonClicked(TempIndex));
                }

                button.interactable = true;
            }
        }
    }

    void OnPerkSelectButtonClicked(int index)//�̋� �� �ε����� �޴´�.
    {

        if (currentPerkIndexA == -1)
        {
            currentPerkIndexA = index;
        }
        else if(currentPerkIndexB == -1)
        {
            currentPerkIndexB = index;
        }
        else
        {
            Debug.Log("���� 2�� �̻� ������ �� �����ϴ�.");
        }
        LastSelectedPerk = index;
        UpdateUI();
    }

    void OnPerkChooseButtonClicked(int index)//�׳� �� ����
    {
        LastSelectedPerk = index;
        UpdateUI();
    }

    void OnPerkBuyButtonClicked(int index)
    {
        if (PlayerData.currentCoin >= DataList.allPerk[index].Cost)
        {
            Debug.Log("�� ���� �Ϸ�");
            PlayerData.currentCoin -= DataList.allPerk[index].Cost;
            PlayerData.AvailablePerk[index] = true;
        }
        else
        {
            Debug.Log("���� ��� �� ���� ����");
        }

        LastSelectedPerk = index;
        UpdateUI();
    }

    void OnPerkDisableButtonClicked(int index)//���� ����
    {
        if(index == currentPerkIndexA)
        {
            currentPerkIndexA = -1;
        }
        else
        {
            currentPerkIndexB = -1;
        }

        LastSelectedPerk = index;
        UpdateUI();
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

    void SyncPlayerData()
    {
        PlayerData.currentPerkIndexA = currentPerkIndexA;
        PlayerData.currentPerkIndexB = currentPerkIndexB;
        PlayerData.currentCharacterIndex = currentCharacterIndex;
        Debug.Log("������ ����ȭ");
    }

}
