using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingMissile : MonoBehaviour
{
    public float speed = 20f;
    public float rotateSpeed = 5;

    private Rigidbody rb;

    private Transform target;

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    public void LaunchAtTarget(Transform target, Transform startAt)
    {
        this.target = target;        
        transform.position = startAt.position;
        transform.rotation = Quaternion.LookRotation(startAt.forward);
        gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
            DestroyTarget();
    }

    private void DestroyTarget()
    {
        rb.velocity = transform.forward * speed;

        //missile goes straight if target is not set
        if (target != null)
        {
            //rotate till missile and target are facing each other
            if (Vector3.Dot(target.forward, transform.forward) < 0)
            {
                Quaternion taretRotation = Quaternion.LookRotation(target.position - rb.position);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, taretRotation, rotateSpeed));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string targetTag = Tags.EnemyTag;
        if(target != null)
            targetTag = target.tag;

        if (other.CompareTag(targetTag))
        {
            gameObject.SetActive(false);
            GameManager.Instance.MissileHitTarget(other.transform, transform.position);
        }
    }
}
