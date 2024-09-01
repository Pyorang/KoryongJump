using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected int point;
    [SerializeField] protected int coin;

    public abstract void Interact(GameObject player);

    public void DeleteItem() { Destroy(this.gameObject); }
    public int GetCoin() { return coin; }
    public int GetPoint() { return point; }
}
