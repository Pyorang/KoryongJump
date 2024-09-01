using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float ORIGINAL_JUMP_POWRER = 8f;

    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isInvincible = false;
    [SerializeField] private int life = 3;
    [SerializeField] private int maxLife = 3;
    [SerializeField] private int earnCoin;
    [SerializeField] private float jumpPower = 1;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float boostTime;
    [SerializeField] private float magneticTime;
    [SerializeField] private float umbrellaTime;

    [SerializeField] private GameObject magneticField;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private ObjectPool pool;

    private Coroutine boostCoroutine;
    private Coroutine magneticCoroutine;
    private Coroutine umbrellaCoroutine;
    private Coroutine potionCoroutine;
    private Coroutine shoesCoroutine;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        
    }

    public bool GetInvincible() { return isInvincible; }
    public bool GetIsDead() { return isDead; }
    public void SetIsDead(bool dead) { isDead = dead; }
    public void ResetLife() 
    { 
        life = maxLife; 

        for(int i = 1; i<= maxLife; i++)
        {
            healthUI.ChangeHealthPlusUI(i);
        }
    }
    public void ResetCoin() { earnCoin = 0; }
    public void ResetLocation() { this.transform.position = new Vector2(0, 0); }
    public void Move()
    {
        if(!isDead)
        {
            float x = Input.GetAxis("Horizontal");

            rb.velocity += new Vector2(x, 0);
        }
    }
    public void LowJump() { rb.velocity = new Vector2(rb.velocity.x, 0.8f * jumpPower); }
    public void HighJump(){ rb.velocity = new Vector2(rb.velocity.x, 1.5f * jumpPower); }
    public void Jump() { rb.velocity = new Vector2(rb.velocity.x, jumpPower ); }
    public void SetVelocityZero() { rb.velocity = Vector2.zero; }
    public int GetEarnCoins() { return earnCoin; }
    public void GetHeal()
    {
        if (life < maxLife)
        {
            life++;
            healthUI.ChangeHealthPlusUI(life);
        }
        else
            return;
    }
    public void GetHurt()
    {
        if (life > 0)
        {
            healthUI.ChangeHealthMinusUI(life);
            life--;
        }

        if (life == 0)
            isDead = true;
    }
    public void EarnCoin(int coin) { earnCoin += coin; }
    
    public void DropCoins()
    {
        int dropCoin = earnCoin;
        if (dropCoin > 10) dropCoin = 10;

        earnCoin -= dropCoin;

        for(int i = 0; i< dropCoin; i++)
        {
            GameObject droppedCoin = pool.GetCoinFromPool();
            droppedCoin.transform.position = this.transform.position;
            droppedCoin.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            float angle = i * (360f / dropCoin);
            float radians = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            droppedCoin.GetComponent<CircleCollider2D>().enabled = false;
            droppedCoin.GetComponent<Rigidbody2D>().velocity = direction.normalized * 2f;
        }
    }

    public void SetJumpPower(float Value) { jumpPower = Value; }

    public void BoostUpOn()
    {
        if(boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        rb.gravityScale = 0;
        isInvincible = true;
        boostCoroutine = StartCoroutine(BoostOff());
    }

    public void MegaBoostUpOn()
    {
        if(boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        rb.gravityScale = 0;
        rb.velocity = new Vector2 (0, 1.5f * boostSpeed);
        isInvincible = true;
        boostCoroutine = StartCoroutine(BoostOff());
    }

    public void MagneticOn()
    {
        if(magneticCoroutine != null)
            StopCoroutine(magneticCoroutine);

        magneticField.SetActive(true);
        magneticCoroutine = StartCoroutine(MagneticOff());
    }

    public void UmbrellaOn(GameObject item)
    {
        if(umbrellaCoroutine != null)
            StopCoroutine (umbrellaCoroutine);

        rb.gravityScale = item.GetComponent<Umbrella>().GetItemGravityScale();
        umbrellaCoroutine = StartCoroutine(UmbrellaOff());
    }

    public void ShacklesOn(GameObject item)
    {
        if(umbrellaCoroutine != null)
            StopCoroutine(umbrellaCoroutine);

        rb.gravityScale = item.GetComponent<Shackles>().GetItemGravityScale();
        umbrellaCoroutine = StartCoroutine(ShacklesOff());
    }

    public void ShrinkPotionOn(GameObject item)
    {
        if (potionCoroutine != null)
            StopCoroutine(potionCoroutine);

        ShrinkPotion sp = item.GetComponent<ShrinkPotion>();
        sp.Interact(this.gameObject);
        potionCoroutine = StartCoroutine(ShrinkPotionOff(sp.GetShrinkTime()));
    }

    public void ShoesOn(GameObject item)
    {
        if (shoesCoroutine != null)
            StopCoroutine(shoesCoroutine);

        Shoes sh = item.GetComponent<Shoes>();
        sh.Interact(this.gameObject);
        shoesCoroutine = StartCoroutine(ShoesOff(sh.GetShoesOnTime()));
    }

    public void KillMonster(GameObject monster)
    {
        pool.ReturnToMonsterPool(monster);
    }

    public void DestroyFootHold(GameObject footHold)
    {
        pool.ReturnToFootHoldPool(footHold);
    }

    public void DestroyThronFootHold(GameObject thronFootHold)
    {
        pool.ReturnToThronFootHoldPool(thronFootHold);
    }

    IEnumerator BoostOff()
    {
        float elapsedTime = 0f;

        while (elapsedTime < boostTime && isInvincible)
        {
            rb.velocity = new Vector2(rb.velocity.x, boostSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = 1;
        isInvincible = false;
    }

    IEnumerator MegaBoostOff()
    {
        float elapsedTime = 0f;

        while (elapsedTime < boostTime && isInvincible)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1.5f * boostSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = 1;
        isInvincible = false;
    }

    IEnumerator MagneticOff()
    {
        yield return new WaitForSeconds(magneticTime);
        magneticField.SetActive(false);
    }

    IEnumerator UmbrellaOff()
    {
        yield return new WaitForSeconds(umbrellaTime);
        rb.gravityScale = 1f;
    }

    IEnumerator ShacklesOff()
    {
        yield return new WaitForSeconds(umbrellaTime);
        rb.gravityScale = 1f;
    }

    IEnumerator ShrinkPotionOff(float shrinkTime)
    {
        yield return new WaitForSeconds(shrinkTime);
        this.gameObject.transform.localScale = new Vector2(1f, 1f);
    }

    IEnumerator ShoesOff(float shoesOnTime)
    {
        yield return new WaitForSeconds(shoesOnTime);
        SetJumpPower(ORIGINAL_JUMP_POWRER);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(!isDead)
        {
            switch (other.gameObject.tag)
            {
                case "Ground":
                    Jump();
                    break;

                case "Coin":
                    other.gameObject.GetComponent<Coin>().Interact(this.gameObject);
                    pool.ReturnToCoinPool(other.gameObject);
                    break;

                case "Star":
                    if(!isInvincible)
                        HighJump();
                    pool.ReturnToStarPool(other.gameObject);
                    break;

                case "Booster":
                    BoostUpOn();
                    pool.ReturnToBoosterPool(other.gameObject);
                    break;

                case "Mega Booster":
                    MegaBoostUpOn();
                    pool.ReturnToMegaBoosterPool(other.gameObject);
                    break;

                case "Magnetic":
                    if(!isInvincible)
                        Jump();
                    MagneticOn();
                    pool.ReturnToMagneticPool(other.gameObject);
                    break;

                case "Umbrella":
                    if(!isInvincible)
                        Jump();
                    UmbrellaOn(other.gameObject);
                    pool.ReturnToUmbrellaPool(other.gameObject);
                    break;

                case "Shackles":
                    if(!isInvincible)
                    {
                        Jump();
                        ShacklesOn(other.gameObject);
                    }
                    pool.ReturnToShacklesPool(other.gameObject);
                    break;

                case "ShrinkPotion":
                    if(!isInvincible)
                        Jump();
                    ShrinkPotionOn(other.gameObject);
                    pool.ReturnToShrinkPotionPool(other.gameObject);
                    break;

                case "Shoes":
                    if(!isInvincible)
                        Jump();
                    ShoesOn(other.gameObject);
                    pool.ReturnToShoesPool(other.gameObject);
                    break;

                case "Monster":
                    Monster monster = other.gameObject.GetComponent<Monster>();
                    if (monster.CheckPlayerUp(this.gameObject))
                    {
                       Jump();
                       KillMonster(other.gameObject);
                    }
                    else if (isInvincible)
                    {
                        KillMonster(other.gameObject);
                    }
                    else
                    {
                        GetHurt();
                        DropCoins();
                    }
                    break;

                case "FootHold":
                    FootHold footHold = other.gameObject.GetComponent<FootHold>();
                    if (footHold.CheckPlayerUp(this.gameObject))
                    {
                        LowJump();
                        DestroyFootHold(other.gameObject);
                    }

                    else if (isInvincible)
                    {
                        DestroyFootHold(other.gameObject);
                    }
                    break;

                case "ThornFootHold":
                    ThornFootHold thornFootHold = other.gameObject.GetComponent<ThornFootHold>();

                    if (thornFootHold.CheckPlayerUp(this.gameObject))
                    {
                        LowJump();
                        GetHurt();
                        DropCoins();
                    }
                    else if (isInvincible)
                    {
                        DestroyThronFootHold(other.gameObject);
                    }
                    break;
            }
        }
    }
}
