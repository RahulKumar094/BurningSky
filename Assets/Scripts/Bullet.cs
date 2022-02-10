using UnityEngine;

public abstract class Bullet
{
    public Transform transform;
    public bool alive;
    public float speed;

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
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.gameObject.SetActive(true);
    }

    public virtual void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
    }
}

public class PlayerBullet : Bullet
{
    public PlayerBullet(Transform transform) : base(transform)
    {
        speed = 50f;
    }
}

public class EnemyBullet : Bullet
{
    public EnemyBullet(Transform transform) : base(transform)
    {
        speed = 30f;
    }
}
