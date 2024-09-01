using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLine : MonoBehaviour
{
    [SerializeField] private ObjectPool pool;

    private Coroutine deathCoroutine;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Player>().SetIsDead(true);
            collision.gameObject.GetComponent<Player>().SetVelocityZero();

            StopDestroyCoroutine();

            deathCoroutine = StartCoroutine(DestroyPlayer(collision.gameObject));
        }

        switch (collision.gameObject.tag)
        {
            case "Booster":
                pool.ReturnToBoosterPool(collision.gameObject);
                break;
            case "Coin":
                pool.ReturnToCoinPool(collision.gameObject);
                break;
            case "FootHold":
                pool.ReturnToFootHoldPool(collision.gameObject);
                break;
            case "Magnetic":
                pool.ReturnToMagneticPool(collision.gameObject);
                break;
            case "Mega Booster":
                pool.ReturnToMegaBoosterPool(collision.gameObject);
                break;
            case "Monster":
                pool.ReturnToMonsterPool(collision.gameObject);
                break;
            case "Shackles":
                pool.ReturnToShacklesPool(collision.gameObject);
                break;
            case "Shoes":
                pool.ReturnToShoesPool(collision.gameObject);
                break;
            case "ShrinkPotion":
                pool.ReturnToShrinkPotionPool(collision.gameObject);
                break;
            case "Star":
                pool.ReturnToStarPool(collision.gameObject);
                break;
            case "ThronFootHold":
                pool.ReturnToThronFootHoldPool(collision.gameObject);
                break;
            case "Umbrella":
                pool.ReturnToUmbrellaPool(collision.gameObject);
                break;
        }
    }

    public void StopDestroyCoroutine() 
    {
        if (deathCoroutine != null)
            StopCoroutine(deathCoroutine); 
    }

    IEnumerator DestroyPlayer(GameObject player)
    {
        yield return new WaitForSeconds(5f);
        player.SetActive(false);
    }
}
