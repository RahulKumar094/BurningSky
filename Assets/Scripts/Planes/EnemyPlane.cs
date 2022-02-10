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
    public float shootDelayAtStart = 0.75f;

    public bool invinsible { get; protected set; }
    public bool alive { get; set; }
    public float health { get; set; }
    public float moveSpeed { get; set; }
    public float fireRate { get; set; }

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

}
