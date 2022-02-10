using System.Collections;
using UnityEngine;

public class MediumEnemyPlane : EnemyPlane
{
    protected enum MediumPlaneState
    {
        MoveToVantage,
        AtVantage,
        SuicideBomb
    }

    public Transform Weapon_Left;
    public Transform Weapon_Right;

    public Vector3 vantagePosition = new Vector3(0f, -20f, 10f);

    protected MediumPlaneState currentState;

    public override void SpawnAt(Vector3 spawnPosition)
    {
        base.SpawnAt(spawnPosition);

        currentState = MediumPlaneState.MoveToVantage;
        StartCoroutine("Shoot");
    }

    public override void Update()
    {
        if (alive)
        {
            Move();
        }
    }

    public override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(shootDelayAtStart);
        while (alive)
        {
            GameManager.Instance.ShootEnemyBullet(Weapon_Left.position, Weapon_Left.forward);
            GameManager.Instance.ShootEnemyBullet(Weapon_Right.position, Weapon_Right.forward);
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag(Tags.PlayerBulletTag))
        {
            GameManager.Instance.EnemyCollideWithBullet(other.transform, this);
        }
    }
}