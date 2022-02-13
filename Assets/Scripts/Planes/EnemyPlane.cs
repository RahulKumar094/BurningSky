using UnityEngine;

public enum EnemyType
{
    NONE,
    SmallRed,
    SmallGreen,
    MediumBlue,
    MediumGrey,
    Boss
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyPlane : MonoBehaviour, IAttribute
{
    public EnemyType type = EnemyType.NONE;
    public PlaneAttribute attribute;
    public Enemy_CoinValue coinValue;
    public float shootDelayAtStart = 0.75f;

    public static bool CanShoot;
    public bool invinsible { get; protected set; }
    public bool alive { get; set; }
    public float health { get; set; }
    public float moveSpeed { get; set; }
    public float fireRate { get; set; }

    private float shootTimer;
    private float maxHealth;

    public void SetAttribute(PlaneAttribute attribute)
    {
        health = attribute.health;
        moveSpeed = attribute.moveSpeed;
        fireRate = attribute.fireRate;
    }

    public virtual void SpawnAt(Vector3 spawnPosition)
    {
        alive = true;
        invinsible = true;
        maxHealth = health;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        gameObject.SetActive(true);

        SetAttribute(attribute);
        shootTimer = shootDelayAtStart;
    }

    protected virtual void Update()
    {
        if (GameManager.Paused) return;

        if (alive)
        {
            Move();

            if(CanShoot)
                Shoot();
        }
    }

    protected virtual void Move()
    {
    }

    private void Shoot()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            ShootAtTarget();
            shootTimer = 1 / fireRate;
        }
    }

    protected virtual void ShootAtTarget()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.InvinsibilityWall))
        {
            invinsible = false;
        }
    }

    public virtual void Destroy()
    {
        alive = false;
        gameObject.SetActive(false);
    }

    public float GetHealthPercent()
    {
        return health / maxHealth * 100f;
    }

    public int GetTotalCoinValue()
    {
        int value = 0;
        foreach (CoinValue coin in coinValue.coinValue)
        {
            value += Coin.GetCoinValue(coin.type) * coin.count;
        }
        return value;
    }
}
