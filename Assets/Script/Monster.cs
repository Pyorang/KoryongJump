using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected int point;
    [SerializeField] protected int coin;

    public bool CheckPlayerUp(GameObject player)
    {
        if (player.transform.position.y > this.transform.position.y)
            return true;
        else
            return false;
    }
}
