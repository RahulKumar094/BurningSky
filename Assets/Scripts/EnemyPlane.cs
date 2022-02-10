using System.Collections;
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

    public bool alive { get; set; }
    public float health { get; set; }
    public float moveSpeed { get; set; }
    public float fireRate { get; set; }

    public void SetAttribute(PlaneAttribute attribute)
    {
        this.health = attribute.health;
        this.moveSpeed = attribute.moveSpeed;
        this.fireRate = attribute.fireRate;
    }

    public virtual void SpawnAt(Vector3 spawnPosition)
    {
        alive = true;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        gameObject.SetActive(true);
    }

    public virtual void Update()
    {
    }

    public virtual void Move()
    {
    }

    public virtual IEnumerator Shoot()
    {
        yield return null;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
    }

    public virtual void Destroy()
    {
        alive = false;
        gameObject.SetActive(false);
    }

}
