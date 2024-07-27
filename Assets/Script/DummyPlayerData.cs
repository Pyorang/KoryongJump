using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerData : MonoBehaviour//Singleton으로 받아올 예정
{

    public int currentCoin;


    public List<bool> AvailableCharacter;
    public List<bool> AvailablePerk;

    public CharacterScript currentCharacter;
    public List<PerkScript> currentPerks;

    public int currentCharacterIndex;
    public int currentPerkIndexA;
    public int currentPerkIndexB;

    public bool dataInitialized;

    [SerializeField]
    DummyDataList dt;


    void InitializeData()//임시 초기화 함수
    {

        AvailableCharacter = new List<bool>();
        AvailablePerk = new List<bool>();

        currentPerks = new List<PerkScript>(2);

        foreach (CharacterScript script in dt.allCharacter) {
            AvailableCharacter.Add(script.defaultAvailable);
        }

        foreach (PerkScript script in dt.allPerk) { AvailablePerk.Add(script.defaultAvailable); }

        currentCharacter = dt.allCharacter[0];
        currentPerks[0] = dt.allPerk[0];
        currentPerks[1] = dt.allPerk[1];//초기값 지정

        currentCharacterIndex = 0;
        currentPerkIndexA = 0;
        currentPerkIndexB = 1; 

        

        dataInitialized = true;
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (!dataInitialized)
        {
            InitializeData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
