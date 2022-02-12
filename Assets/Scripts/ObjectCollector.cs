using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PlayerBulletTag) || other.CompareTag(Tags.EnemyBulletTag))
        {
            ObjectPool.Instance.DestroyBullet(other.transform);
        }
        else if (other.CompareTag(Tags.EnemyTag))
        {
            EnemyPlane plane = other.GetComponent<EnemyPlane>();
            plane.Destroy();
        }
        else if (other.CompareTag(Tags.HomingMissile))
        {
            other.gameObject.SetActive(false);
        }
    }
}
