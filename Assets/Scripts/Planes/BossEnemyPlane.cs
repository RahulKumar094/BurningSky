using UnityEngine;

public class BossEnemyPlane : EnemyPlane
{
    protected enum BossPlaneState
    {
        MoveToVantage,
        AtVantage,
    }

    public Transform Weapon_Left;
    public Transform Weapon_Center;
    public Transform Weapon_Right;
    public Transform Tail;

    public Vector3 vantagePosition = new Vector3(0f, -20f, 10f);

    public MinMax XBound;
    public MinMax YBound;
    public float switchPositionAfterSecs = 2;
    public float tailSpinSpeed = 50f;

    protected BossPlaneState currentState;

    private float timer;

    public override void SpawnAt(Vector3 spawnPosition)
    {
        base.SpawnAt(spawnPosition);

        currentState = BossPlaneState.MoveToVantage;
        StartCoroutine("Shoot");
    }

    protected override void Update()
    {
        base.Update();

        if (alive)
        {
            AnimateTail();
        }
    }

    private void AnimateTail()
    {
        Tail.rotation *= Quaternion.Euler(Vector3.forward * tailSpinSpeed);
    }

    protected override void Move()
    {
        switch (currentState)
        {
            case BossPlaneState.MoveToVantage:
                {
                    timer = 0f;
                    transform.position = Vector3.MoveTowards(transform.position, vantagePosition, moveSpeed * Time.deltaTime);
                    if (transform.position == vantagePosition)
                    {
                        currentState = BossPlaneState.AtVantage;
                    }
                    break;
                }
            case BossPlaneState.AtVantage:
                {
                    timer += Time.deltaTime;
                    if (timer >= switchPositionAfterSecs)
                    {
                        GameManager.Instance.EnemyLaunchMissile(Weapon_Center);

                        vantagePosition = new Vector3(Random.Range(XBound.min, XBound.max), -20f, Random.Range(YBound.min, YBound.max));
                        currentState = BossPlaneState.MoveToVantage;
                    }
                    break;
                }
        }
    }

    protected override void ShootAtTarget()
    {
        Vector3 targetDirection = GameManager.Instance.PlayerTransform.position - Weapon_Left.position;
        GameManager.Instance.ShootEnemyBullet(Weapon_Left.position, targetDirection.normalized);

        targetDirection = GameManager.Instance.PlayerTransform.position - Weapon_Right.position;
        GameManager.Instance.ShootEnemyBullet(Weapon_Right.position, targetDirection.normalized);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag(Tags.PlayerBulletTag))
        {
            GameManager.Instance.EnemyCollideWithBullet(other.transform, this);
        }
    }
}