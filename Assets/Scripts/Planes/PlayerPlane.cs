using System;
using System.Collections;
using UnityEngine;

public class PlayerPlane : MonoBehaviour, IAttribute
{
    public Transform Weapon_Left;
    public Transform Weapon_Right;

    public MinMax XBound = new MinMax(-8.8f, 8.8f);
    public MinMax ZBound = new MinMax(-15.2f, 15.2f);

    private readonly Vector3 startPosition = new Vector3(0, -20, -8);

    private bool canMove;

    public bool alive { get; set; }
    public float health { get ; set; }
    public float moveSpeed { get ; set ; }
    public float fireRate { get ; set ; }

    public void SetAttribute(PlaneAttribute attribute)
    {
        health = attribute.health;
        moveSpeed = attribute.moveSpeed;
        fireRate = attribute.fireRate;
    }

    public void SpawnWithCutscene(Action OnCompleteCallback)
    {
        alive = true;
        gameObject.SetActive(true);
        transform.position = Vector3.zero;
        StartCoroutine(MoveToStartCoroutine(OnCompleteCallback));
    }

    private IEnumerator MoveToStartCoroutine(Action OnCompleteCallback)
    {
        canMove = false;

        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 15f * Time.deltaTime);
            yield return null;
        }

        canMove = true;
        OnCompleteCallback?.Invoke();

        StartCoroutine("Shoot");
    }

    public void Move(Vector2 direction)
    {
        if (canMove)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
            transform.position = Vector3.Lerp(transform.position, transform.position + moveDirection, moveSpeed * Time.deltaTime);

            Vector3 finalPosition = transform.position;
            finalPosition.x = Mathf.Clamp(finalPosition.x, XBound.min, XBound.max);
            finalPosition.z = Mathf.Clamp(finalPosition.z, ZBound.min, ZBound.max);
            transform.position = finalPosition;
        }
    }

    public IEnumerator Shoot()
    {
        while (health > 0)
        {
            GameManager.Instance.ShootPlayerBullet(Weapon_Left.position, Weapon_Left.forward);
            GameManager.Instance.ShootPlayerBullet(Weapon_Right.position, Weapon_Right.forward);
            yield return new WaitForSeconds(1 / fireRate);
        }
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
    }

    public void Destroy()
    {
        alive = false;
        canMove = false;
        gameObject.SetActive(false);
    }
}

public struct MinMax
{
    public float min;
    public float max;
    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}