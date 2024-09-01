using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject boosterPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject footHoldPrefab;
    [SerializeField] private GameObject magneticPrefab;
    [SerializeField] private GameObject megaBoosterPrefab;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject shacklesPrefab;
    [SerializeField] private GameObject shoesPrefab;
    [SerializeField] private GameObject shrinkPotionPrefab;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject thronFootHoldPrefab;
    [SerializeField] private GameObject umbrellaPrefab;

    private Queue<GameObject> boosterPool = new Queue<GameObject>();
    private Queue<GameObject> coinPool = new Queue<GameObject>();
    private Queue<GameObject> footHoldPool = new Queue<GameObject>();
    private Queue<GameObject> magneticPool = new Queue<GameObject>();
    private Queue<GameObject> megaBoosterPool = new Queue<GameObject>();
    private Queue<GameObject> monsterPool = new Queue<GameObject>();
    private Queue<GameObject> shacklesPool = new Queue<GameObject>();
    private Queue<GameObject> shoesPool = new Queue<GameObject>();
    private Queue<GameObject> shrinkPotionPool = new Queue<GameObject>();
    private Queue<GameObject> starPool = new Queue<GameObject>();
    private Queue<GameObject> thronFootHoldPool = new Queue<GameObject>();
    private Queue<GameObject> umbrellaPool = new Queue<GameObject>();


    void Start()
    {
        InstantiateAllPool();
    }

    public void InstantiateAllPool()
    {
        InstantiatePool(boosterPrefab, boosterPool, 10);
        InstantiatePool(coinPrefab, coinPool, 100);
        InstantiatePool(footHoldPrefab, footHoldPool, 60);
        InstantiatePool(magneticPrefab, magneticPool, 10);
        InstantiatePool(megaBoosterPrefab, megaBoosterPool, 10);
        InstantiatePool(monsterPrefab, monsterPool, 40);
        InstantiatePool(shacklesPrefab, shacklesPool, 10);
        InstantiatePool(shoesPrefab, shoesPool, 10);
        InstantiatePool(shrinkPotionPrefab, shrinkPotionPool, 10);
        InstantiatePool(starPrefab, starPool, 50);
        InstantiatePool(thronFootHoldPrefab, thronFootHoldPool, 40);
        InstantiatePool(umbrellaPrefab, umbrellaPool, 10);
    }

    public void InstantiatePool(GameObject prefab, Queue<GameObject> pool, int poolSize)
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBoosterFromPool()
    {
        if (boosterPool.Count > 0)
        {
            GameObject obj = boosterPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(boosterPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetCoinFromPool()
    {
        if (coinPool.Count > 0)
        {
            GameObject obj = coinPool.Dequeue();
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(coinPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetFootHoldFromPool()
    {
        if (footHoldPool.Count > 0)
        {
            GameObject obj = footHoldPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(footHoldPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetMagneticFromPool()
    {
        if (magneticPool.Count > 0)
        {
            GameObject obj = magneticPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(magneticPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetMegaBoosterFromPool()
    {
        if (megaBoosterPool.Count > 0)
        {
            GameObject obj = megaBoosterPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(megaBoosterPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetMonsterFromPool()
    {
        if (monsterPool.Count > 0)
        {
            GameObject obj = monsterPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(monsterPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetShacklesFromPool()
    {
        if (shacklesPool.Count > 0)
        {
            GameObject obj = shacklesPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(shacklesPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetShoesFromPool()
    {
        if (shoesPool.Count > 0)
        {
            GameObject obj = shoesPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(shoesPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetShrinkPotionFromPool()
    {
        if (shrinkPotionPool.Count > 0)
        {
            GameObject obj = shrinkPotionPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(shrinkPotionPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetStarFromPool()
    {
        if (starPool.Count > 0)
        {
            GameObject obj = starPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(starPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetThronFootHoldFromPool()
    {
        if (thronFootHoldPool.Count > 0)
        {
            GameObject obj = thronFootHoldPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(thronFootHoldPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public GameObject GetUmbrellaFromPool()
    {
        if (umbrellaPool.Count > 0)
        {
            GameObject obj = umbrellaPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(umbrellaPrefab);
            obj.transform.parent = this.transform;
            return obj;
        }
    }

    public void ReturnToBoosterPool(GameObject obj)
    {
        obj.SetActive(false);
        boosterPool.Enqueue(obj);
    }

    public void ReturnToCoinPool(GameObject obj)
    {
        obj.SetActive(false);
        coinPool.Enqueue(obj);
    }

    public void ReturnToFootHoldPool(GameObject obj)
    {
        obj.SetActive(false);
        footHoldPool.Enqueue(obj);
    }

    public void ReturnToMagneticPool(GameObject obj)
    {
        obj.SetActive(false);
        magneticPool.Enqueue(obj);
    }

    public void ReturnToMegaBoosterPool( GameObject obj)
    {
        obj.SetActive(false);
        megaBoosterPool.Enqueue(obj);
    }

    public void ReturnToMonsterPool(GameObject obj)
    {
        obj.SetActive(false);
        monsterPool.Enqueue(obj);
    }

    public void ReturnToShacklesPool(GameObject obj)
    {
        obj.SetActive(false);
        shacklesPool.Enqueue(obj);
    }

    public void ReturnToShoesPool(GameObject obj)
    {
        obj.SetActive(false);
        shoesPool.Enqueue(obj);
    }

    public void ReturnToShrinkPotionPool(GameObject obj)
    {
        obj.SetActive(false);
        shrinkPotionPool.Enqueue(obj);
    }

    public void ReturnToStarPool(GameObject obj)
    {
        obj.SetActive(false);
        starPool.Enqueue(obj);
    }

    public void ReturnToThronFootHoldPool(GameObject obj)
    {
        obj.SetActive(false);
        thronFootHoldPool.Enqueue(obj);
    }

    public void ReturnToUmbrellaPool(GameObject obj)
    {
        obj.SetActive(false);
        umbrellaPool.Enqueue(obj);
    }

    public void DestroyAllInPool()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach(var obj in allChildren)
        {
            obj.position = this.transform.position;
            obj.gameObject.SetActive(false);
        }

        this.gameObject.SetActive(true);
    }
}
