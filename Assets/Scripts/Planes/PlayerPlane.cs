using System;
using System.Collections;
using UnityEngine;

public class PlayerPlane : MonoBehaviour, IAttribute
{
    public PlaneAttribute Attribute;
    public Transform Weapon_Left;
    public Transform Weapon_Center;
    public Transform Weapon_Right;

    public GameObject Shield;

    public MinMax XBound = new MinMax(-8.8f, 8.8f);
    public MinMax ZBound = new MinMax(-15.2f, 15.2f);

    private readonly Vector3 startPosition = new Vector3(0, -20, -8);
    

    public bool alive { get; set; }
    public float health { get ; set; }
    public float moveSpeed { get ; set ; }
    public float fireRate { get ; set ; }

    public bool ShieldActive { get { return Shield.activeSelf; } }
    public float GetHealthPercentage { get { return health / maxHealth * 100f; } }
    public int MissileCount { get { return missileLauncher.Count; } }
    public float MissileRechargeProgress { get { return missileLauncher.rechargeProgress; } }
    public int ShieldCount { get { return shield.Count; } }
    public float ShieldRechargeProgress { get { return shield.rechargeProgress; } }

    private bool canMove;
    private bool canShoot;    
    private float maxHealth;    
    private float shootTimer;
    private PowerUp missileLauncher;
    private PowerUp shield;

    public void SetAttribute(PlaneAttribute attribute)
    {
        health = attribute.health;
        moveSpeed = attribute.moveSpeed;
        fireRate = attribute.fireRate;
    }

    public void SpawnWithCutscene(Action OnCompleteCallback)
    {
        SetAttribute(Attribute);
        Shield.SetActive(false);

        maxHealth = health;
        gameObject.SetActive(true);
        transform.position = Vector3.zero;
        
        CreatePowerUps();
        StartCoroutine(MoveToStartCoroutine(OnCompleteCallback));
    }

    private IEnumerator MoveToStartCoroutine(Action OnCompleteCallback)
    {
        canMove = false;
        canShoot = false;

        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 10f * Time.deltaTime);
            yield return null;
        }

        alive = true;
        canMove = true;
        canShoot = true;
        shootTimer = 1 / fireRate;

        OnCompleteCallback?.Invoke();
    }

    public void Move(Vector2 direction, float sensitivity)
    {
        if (canMove)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
            transform.position = Vector3.Lerp(transform.position, transform.position + moveDirection, sensitivity * moveSpeed * Time.deltaTime);

            Vector3 finalPosition = transform.position;
            finalPosition.x = Mathf.Clamp(finalPosition.x, XBound.min, XBound.max);
            finalPosition.z = Mathf.Clamp(finalPosition.z, ZBound.min, ZBound.max);
            transform.position = finalPosition;
        }
    }

    private void Shoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            GameManager.Instance.ShootPlayerBullet(Weapon_Left.position, Weapon_Left.forward);
            GameManager.Instance.ShootPlayerBullet(Weapon_Right.position, Weapon_Right.forward);
            shootTimer = 1 / fireRate;
        }
    }

    private void Update()
    {
        if (GameManager.Paused) return;

        if (alive && canShoot) Shoot();

        //update powerUps
        missileLauncher.Update();
        shield.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.EnemyBulletTag))
        {
            GameManager.Instance.PlayerCollideWithBullet(other.transform);
        }
        else if (other.CompareTag(Tags.EnemyTag))
        {
            EnemyPlane plane = other.GetComponent<EnemyPlane>();
            GameManager.Instance.PlayerCollideWithEnemy(plane, other.ClosestPoint(plane.transform.position));
        }
        else if (other.CompareTag(Tags.CoinTag))
        {
            Coin coin = other.GetComponent<Coin>();
            GameManager.Instance.PlayerCollectCoin(coin.CoinType);
            coin.Destroy();
        }
    }

    private void CreatePowerUps()
    {
        missileLauncher = new PowerUp();
        missileLauncher.Initialize(3, 2f, 8f);

        shield = new PowerUp();
        shield.Initialize(1, 10f, 20f, () => { Shield.SetActive(false); });
    }

    public void ActivateMissile(Action OnActivationCallback)
    {
        missileLauncher.Activate(OnActivationCallback);
    }

    public void ActivateShield()
    {
        shield.Activate(() => Shield.SetActive(true));
    }

    public void Destroy()
    {
        alive = false;
        canMove = false;
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct MinMax
{
    [SerializeField]
    public float min;
    [SerializeField]
    public float max;
    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

class PowerUp
{
    public int maxCount;
    public float coolDownInSec;
    public float rechargeTimeInSec;
    public int Count { get { return count; } }

    public float rechargeProgress 
    { 
        get
        {
            if (count == maxCount) return 1;
            return (rechargeTimeInSec - rechargeTimer) / rechargeTimeInSec;
        }
    }

    private int count;
    private bool canActivate;
    private float coolDownTimer;
    private float rechargeTimer;

    private Action CooldownCallback;

    public void Initialize(int maxCount, float coolDownInSec, float rechargeTimeInSec, Action OnCooldownComplete = null)
    {
        this.maxCount = maxCount;
        this.coolDownInSec = coolDownInSec;
        this.rechargeTimeInSec = rechargeTimeInSec;

        count = maxCount;
        canActivate = true;
        coolDownTimer = 0f;
        rechargeTimer = 0f;
        CooldownCallback = OnCooldownComplete;
    }

    public void Activate(Action OnActivationCallback)
    {
        if (canActivate && count > 0)
        {
            count--;
            canActivate = false;
            coolDownTimer = coolDownInSec;

            if(rechargeTimer == 0f)
                rechargeTimer = rechargeTimeInSec;

            OnActivationCallback?.Invoke();
        }
    }

    public void Update()
    {
        if (!canActivate)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0)
            {
                canActivate = true;
                CooldownCallback?.Invoke();
            }
        }

        if (count < maxCount)
        {
            rechargeTimer -= Time.deltaTime;
            if (rechargeTimer <= 0)
            {
                count++;
                rechargeTimer = rechargeTimeInSec;
            }
        }
    }
}