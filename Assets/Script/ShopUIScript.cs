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
    public Button[] PerkSelectButton;//퍽 선택 버튼(4개)

    [SerializeField]
    public Image LockPerkImage;

    [SerializeField]
    public TMPro.TextMeshProUGUI currentCoin;

    public DummyPlayerData PlayerData;//CurrentSelectedCharacterIndex, CharacterUnlockedList, PerkUnlockedList, CurrentCoin를 가져온다
    public DummyDataList DataList;// 퍽과 캐릭터 데이터 리스트를 가져온다.

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
        PlayerData = FindObjectOfType<DummyPlayerData>();//플레이어 데이터 가져오기
        if(PlayerData == null)
        {
            Debug.LogError("Player정보를 가져올 수 없습니다.");
        }

        DataList = FindObjectOfType<DummyDataList>();
        if(DataList == null)
        {
            Debug.LogError("데이터를 가져올 수 없습니다.");
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

        //캐릭터 상태 변경
        CharacterName.text = currentCharacter.name;
        SelectedCharacterSprite = currentCharacter.image;


        //구매 버튼 상태 변경
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

        //퍽 상태 변경

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

        /* 퍽 이미지 변경 코드 */


        currentCoin.text = PlayerData.currentCoin.ToString();

    }

    void OnGoToMenuButtonClicked()
    {
        SyncPlayerData();
        //씬 전환 함수
        Debug.Log("메인메뉴로 갔다고 상상중");
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

            Debug.Log("캐릭터 구매함");

        }
        else
        {
            Debug.Log("돈없어서 캐릭터 구매 못함");
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

            int TempIndex = index[i];//AddListener 내부 람다함수의 매개변수 전달 문제 해결을 위한 임시 변수

            if (index[i] == -1)// 존재하지 않는 경우
            {
                text.text = "None";
                button.interactable = false;
            }
            else
            {
                bool isSelected = currentPerkIndexA == index[i] || currentPerkIndexB == index[i];
                bool isAvailable = PlayerData.AvailablePerk[index[i]];
                bool isLastSelected = LastSelectedPerk == index[i];

                if (isSelected)// 이미 선택된 퍽일 경우
                {
                    text.text = "Disable";
                    button.onClick.AddListener(() => OnPerkDisableButtonClicked(TempIndex));
                }
                else if (!isAvailable && !isLastSelected) // 선택되지 않았고, 이용 불가능한 경우
                {
                    text.text = DataList.allPerk[index[i]].Name;
                    button.onClick.AddListener(() => OnPerkChooseButtonClicked(TempIndex));
                }
                else if (!isAvailable) // 선택되지 않은 경우
                {
                    text.text = DataList.allPerk[index[i]].Cost.ToString();
                    button.onClick.AddListener(() => OnPerkBuyButtonClicked(TempIndex));
                }
                else //선택할 수 있는 경우
                {
                    text.text = "Enable";
                    button.onClick.AddListener(() => OnPerkSelectButtonClicked(TempIndex));
                }

                button.interactable = true;
            }
        }
    }

    void OnPerkSelectButtonClicked(int index)//이떄 퍽 인덱스를 받는다.
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
            Debug.Log("퍽을 2개 이상 선택할 수 없습니다.");
        }
        LastSelectedPerk = index;
        UpdateUI();
    }

    void OnPerkChooseButtonClicked(int index)//그냥 퍽 선택
    {
        LastSelectedPerk = index;
        UpdateUI();
    }

    void OnPerkBuyButtonClicked(int index)
    {
        if (PlayerData.currentCoin >= DataList.allPerk[index].Cost)
        {
            Debug.Log("퍽 구매 완료");
            PlayerData.currentCoin -= DataList.allPerk[index].Cost;
            PlayerData.AvailablePerk[index] = true;
        }
        else
        {
            Debug.Log("돈이 없어서 퍽 구매 못함");
        }

        LastSelectedPerk = index;
        UpdateUI();
    }

    void OnPerkDisableButtonClicked(int index)//선택 해제
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
        Debug.Log("캐릭터 변경함");
    }

    void SyncPlayerData()
    {
        PlayerData.currentPerkIndexA = currentPerkIndexA;
        PlayerData.currentPerkIndexB = currentPerkIndexB;
        PlayerData.currentCharacterIndex = currentCharacterIndex;
        Debug.Log("데이터 동기화");
    }

}
