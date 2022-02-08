using System.Collections;
using UnityEngine;

public class Plane : MonoBehaviour, IAttribute
{
    public MinMax XBound = new MinMax(-8.8f, 8.8f);
    public MinMax ZBound = new MinMax(-15.2f, 15.2f);

    private readonly Vector3 startPosition = new Vector3(0, -20, -8);

    private bool canMove;

    public float health { get ; set; }
    public float moveSpeed { get ; set ; }
    public float fireRate { get ; set ; }

    public void SetAttribute(float health, float moveSpeed, float fireRate)
    {
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.fireRate = fireRate;
    }

    public void MoveToStart()
    {
        StartCoroutine("MoveToStartCoroutine");
    }

    private IEnumerator MoveToStartCoroutine()
    {
        canMove = false;

        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, 15f * Time.deltaTime);
            yield return null;
        }

        canMove = true;
        Shoot();
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

    public async void Shoot()
    {
        while (health > 0)
        {
            GameManager.Instance.ShootBullet(transform.position, transform.forward);
            int timeInMilliSec = Mathf.RoundToInt(1 / fireRate * 1000);
            await System.Threading.Tasks.Task.Delay(timeInMilliSec);
        }
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