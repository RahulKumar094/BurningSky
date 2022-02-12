using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    public float lifeTime = 8f;
    public float explosionRadius = 2f;
    public float explosionForce = 100f;

    public CoinType CoinType { get {return type; } }

    private CoinType type;
    private float timer;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Burst(CoinType type, Vector3 startPosition)
    {
        this.type = type;

        timer = lifeTime;
        transform.position = startPosition;
        transform.localScale = (int)type * Vector3.one;
        gameObject.SetActive(true);

        Vector3 forceDirection = Random.insideUnitSphere;
        rb.AddExplosionForce(explosionForce, startPosition + forceDirection, explosionRadius);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    public static int GetCoinValue(CoinType type)
    {
        if (type == CoinType.Small)
            return 1;
        else if (type == CoinType.Medium)
            return 5;
        else if (type == CoinType.Large)
            return 10;

        return 0;
    }
}

public enum CoinType
{
    Small = 1,
    Medium,
    Large
}
