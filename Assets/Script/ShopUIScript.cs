using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Search;
using UnityEngine.SceneManagement;

public class ShopUIScript : MonoBehaviour
{
    [SerializeField]
    public string MainSceneName = "MainScene";
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
    public Button PerkEnableButton;

    [SerializeField]
    public Image LockPerkImage;

    [SerializeField]
    public TMPro.TextMeshProUGUI currentCoin;

    [SerializeField]
    public ScrollRect descriptionScroll;

    public DummyPlayerData PlayerData;//CurrentSelectedCharacterIndex, CharacterUnlockedList, PerkUnlockedList, CurrentCoin�� �����´�
    public DummyDataList DataList;// �ܰ� ĳ���� ������ ����Ʈ�� �����´�.

    public int currentCharacterIndex;
    public int currentPerkIndexA;
    public int currentPerkIndexB;

    public int LastSelectedPerk;
    public int LastUpdatedPerkPosition;    

    public Image[] CharacterImages;
    public Image[] PerkImages;

    public TextMeshProUGUI TempPerkNameTextA;
    public TextMeshProUGUI TempPerkNameTextB;

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

        TempPerkNameTextA = SelectedPerkA.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        TempPerkNameTextB = SelectedPerkB.gameObject.GetComponentInChildren<TextMeshProUGUI>();

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
        UpdatePerkEnableButton();
        if(LastSelectedPerk == -1)
        {
            PerkDescreption.text = "This is Perk Description";
        }
        else
        {
            PerkDescreption.text = DataList.allPerk[LastSelectedPerk].Description;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(PerkDescreption.GetComponent<RectTransform>());
        descriptionScroll.verticalNormalizedPosition = 1f;

        //�� �̹��� ����
        TempPerkNameTextA.gameObject.SetActive(true);
        if (currentPerkA == null)
        {
            TempPerkNameTextA.text = "None";
        }
        else
        {
            if(currentPerkA.image == null)
            {
                TempPerkNameTextA.text = currentPerkA.Name;
            }
            else
            {
                TempPerkNameTextA.gameObject.SetActive(false);
                SelectedPerkA = currentPerkA.image;
            }
        }

        TempPerkNameTextB.gameObject.SetActive(true);
        if (currentPerkB == null)
        {
            TempPerkNameTextB.text = "None";
        }
        else
        {
            if (currentPerkB.image == null)
            {
                TempPerkNameTextB.text = currentPerkB.Name;
            }
            else
            {
                TempPerkNameTextB.gameObject.SetActive(false);
                SelectedPerkB = currentPerkB.image;
            }
        }

        if(currentCharacterIndex != PlayerData.currentCharacterIndex)
        {
            TempPerkNameTextA.gameObject.SetActive(true);
            TempPerkNameTextB.gameObject.SetActive(true);
            TempPerkNameTextA.text = "-";
            TempPerkNameTextB.text = "-";
        }


        currentCoin.text = PlayerData.currentCoin.ToString();

    }

    void OnGoToMenuButtonClicked()
    {
        SyncPlayerData();
        //�� ��ȯ �Լ�
        //Debug.Log("���θ޴��� ���ٰ� �����");
        SceneManager.LoadScene(MainSceneName);
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
                bool isLastSelected = LastSelectedPerk == index[i];

                if (isLastSelected)
                {
                    text.text = "Selected";
                    button.interactable = false;
                }
                else
                {
                    text.text = DataList.allPerk[index[i]].Name;
                    button.onClick.AddListener(()=>OnPerkChooseButtonClicked(TempIndex));
                    button.interactable = true;
                }
            }
        }
    }

    void UpdatePerkEnableButton()
    {


        PerkEnableButton.interactable = true;

        PerkEnableButton.onClick.RemoveAllListeners();
        var text = PerkEnableButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if(LastSelectedPerk == -1)
        {
            text.text = "None";
            PerkEnableButton.interactable = false;
            return;
        }

        bool isSelected = currentPerkIndexA == LastSelectedPerk || currentPerkIndexB == LastSelectedPerk;
        bool isAvailable = PlayerData.AvailablePerk[LastSelectedPerk];

        if (isSelected)
        {
            text.text = "Disable";
            PerkEnableButton.onClick.AddListener(() => OnPerkDisableButtonClicked(LastSelectedPerk));
        }else if (!isAvailable)
        {
            text.text = DataList.allPerk[LastSelectedPerk].Cost.ToString();
            PerkEnableButton.onClick.AddListener(() => OnPerkBuyButtonClicked(LastSelectedPerk));
        }
        else
        {
            text.text = "Enable";
            PerkEnableButton.onClick.AddListener(() => OnPerkSelectButtonClicked(LastSelectedPerk));
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
