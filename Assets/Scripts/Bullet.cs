using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public bool alive { set; get; }

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
