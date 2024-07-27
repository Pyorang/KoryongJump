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
        var currentPerkA = DataList.allPerk[currentPerkIndexA];
        var currentPerkB = DataList.allPerk[currentPerkIndexB];

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




        currentCoin.text = PlayerData.currentCoin.ToString();

    }

    void OnGoToMenuButtonClicked()
    {
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


    void UpdatePerkSelectButton(int[] index) //퍽 인덱스를 받아서 각각 업데이트한다.
    {
        for (int i = 0; i < index.Length; i++)
        {

            PerkSelectButton[i].onClick.RemoveAllListeners();

            if (index[i] == -1)
            {
                //퍽 정보 없음.

                PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "None";
                PerkSelectButton[i].interactable = false;
            }
            else
            {

                if (currentPerkIndexA == index[i] || currentPerkIndexB == index[i])//이미 선택된 퍽인 경우
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkDisableButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Disable";
                }
                else if (!PlayerData.AvailablePerk[index[i]] && LastSelectedPerk != index[i])//없는 퍽이고 선택하지 않은 경우
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkChooseButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = DataList.allPerk[index[i]].Name;
                }
                else if (!PlayerData.AvailablePerk[index[i]])//없는 퍽이고 한번 더 누른 경우
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkBuyButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = DataList.allPerk[index[i]].Cost.ToString();
                }
                else//누른 적 없지만 가지고 있는 퍽인 경우
                {
                    PerkSelectButton[i].onClick.AddListener(delegate { OnPerkSelectButtonClicked(index[i]); });
                    PerkSelectButton[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Enable";
                }

                PerkSelectButton[i].interactable = true;
            }
        }
    }

    void OnPerkSelectButtonClicked(int index)//이떄 퍽 인덱스를 받는다.
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
        Debug.Log("캐릭터 변경함");
    }

}
