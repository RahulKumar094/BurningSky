using System.Collections;
using UnityEngine;

public class SmallEnemyPlane : EnemyPlane
{
    public Transform Weapon;

    //for green plane rotation
    private float rotationSpeed = 50f;
    private float yRotationAngle = 90f;

    private float shootDelay = 0.75f;

    public override void SpawnAt(Vector3 spawnPosition)
    {
        base.SpawnAt(spawnPosition);
        StartCoroutine("Shoot");
    }

    public void SetRotation(float yAxisMultiplier, float delay)
    {
        if (type == EnemyType.SmallGreen)
            StartCoroutine(RotateAtAngle(yAxisMultiplier * yRotationAngle, rotationSpeed, delay));
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
        yield return new WaitForSeconds(shootDelay);

        Vector3 targetDirection = GameManager.Instance.PlayerTransform.position - transform.position;
        GameManager.Instance.ShootEnemyBullet(Weapon.position, targetDirection.normalized);
    }

    private IEnumerator RotateAtAngle(float yAxisRotation, float rotationSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation = new Vector3(eulerRotation.x, eulerRotation.y + yAxisRotation, eulerRotation.z);
        while (alive)
        {
            Vector3 rotation = Vector3.MoveTowards(transform.rotation.eulerAngles, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotation);
            yield return null;
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
