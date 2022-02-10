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

    public override void Move()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);
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
        //this plane fire only once after start delay
        yield return new WaitForSeconds(shootDelayAtStart);

        Vector3 targetDirection = GameManager.Instance.PlayerTransform.position - transform.position;
        GameManager.Instance.ShootEnemyBullet(Weapon.position, targetDirection.normalized);
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
