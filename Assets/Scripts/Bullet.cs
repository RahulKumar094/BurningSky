using UnityEngine;

public abstract class Bullet
{
    public Transform transform;
    public bool alive;
    public float speed;

    private Vector3 direction;

    public Bullet(Transform transform)
    {
        alive = false;
        this.transform = transform;
        transform.gameObject.SetActive(false);
    }

    public virtual void Destroy()
    {
        alive = false;
        transform.gameObject.SetActive(false);
    }

    public virtual void Shoot(Vector3 startPosition, Vector3 direction)
    {
        alive = true;
        this.direction = direction;
        transform.position = startPosition;
        transform.gameObject.SetActive(true);
    }

    public virtual void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}

public class PlayerBullet : Bullet
{
    public PlayerBullet(Transform transform) : base(transform)
    {
        speed = 80f;
    }
}

public class EnemyBullet : Bullet
{
    public EnemyBullet(Transform transform) : base(transform)
    {
        speed = 20f;
    }
}
