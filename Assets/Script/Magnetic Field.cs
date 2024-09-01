using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private float magnetStrength;

    private Transform trans;
    private Rigidbody2D rb;
    [SerializeField] private List<GameObject> attractedCoins;

    private void Start()
    {
        trans = this.transform;
        attractedCoins = new List<GameObject>();
    }

    void Update()
    {
        for(int i = 0; i < attractedCoins.Count; i++)
        {
            Vector2 directionToMagnet = trans.position - attractedCoins[i].transform.position;
            attractedCoins[i].GetComponent<Rigidbody2D>().velocity = directionToMagnet.normalized * magnetStrength;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
            attractedCoins.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
            attractedCoins.Remove(collision.gameObject);
    }
}
