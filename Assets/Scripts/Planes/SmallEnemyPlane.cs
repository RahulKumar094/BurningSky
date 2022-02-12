using System.Collections;
using UnityEngine;

public class SmallEnemyPlane : EnemyPlane
{
    public Transform Weapon;

    public override void SpawnAt(Vector3 spawnPosition)
    {
        base.SpawnAt(spawnPosition);
        StartCoroutine("Shoot");
    }

    protected override void Move()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);
    }

    protected override void ShootAtTarget()
    {
        Vector3 targetDirection = GameManager.Instance.PlayerTransform.position - Weapon.position;
        GameManager.Instance.ShootEnemyBullet(Weapon.position, targetDirection.normalized);
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
