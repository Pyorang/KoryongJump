using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    private int patternStageNum;
    [SerializeField] private float maxHeight;
    [SerializeField] private ObjectPool pool;
    [SerializeField] private List<PatternData> patterns;

    private void Start()
    {
        maxHeight = transform.position.y;
    }

    private void Update()
    {
        UpdateHeightInfo();
        InstantiatePattern();
    }

    public void UpdateHeightInfo()
    {
        if (maxHeight < transform.position.y)
            maxHeight = transform.position.y;
    }

    public void ResetMaxHeight() { maxHeight = 0; }
    public void ResetPattern() 
    {
        patternStageNum = 0;
        pool.DestroyAllInPool(); 
    }

    public void InstantiatePattern(int num)
    {
        foreach (var interactObj in patterns[num].GetObjectsList())
        {
            switch (interactObj.GetObject().tag)
            {
                case "Booster":
                    pool.GetBoosterFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Coin":
                    pool.GetCoinFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "FootHold":
                    pool.GetFootHoldFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Magnetic":
                    pool.GetMagneticFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Mega Booster":
                    pool.GetMegaBoosterFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Monster":
                    pool.GetMonsterFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Shackles":
                    pool.GetShacklesFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Shoes":
                    pool.GetShoesFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "ShrinkPotion":
                    pool.GetShrinkPotionFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Star":
                    pool.GetStarFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "ThronFootHold":
                    pool.GetThronFootHoldFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
                case "Umbrella":
                    pool.GetUmbrellaFromPool().transform.position = interactObj.GetLocation() + transform.position;
                    break;
            }
        }
    }

    public void InstantiatePattern()
    {
        int temp = (int)maxHeight / 10;

        if(temp!= patternStageNum)
            InstantiatePattern(Random.Range(0, patterns.Count));

        patternStageNum = temp;
    }
}
