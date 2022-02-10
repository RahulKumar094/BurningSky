using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get { return instance; } }
    private static ObjectPool instance;

    public int PlayerBulletCount = 50;
    public int EnemyBulletCount = 50;
    public GameObject PlayerBulletPrefab;
    public GameObject EnemyBulletPrefab;
    public Transform PlayerBulletContainer;
    public Transform EnemyBulletContainer;

    public EnemyPoolInfo[] enemyPoolInfo = new EnemyPoolInfo[5];

    public static List<EnemyPlane> EnemyPlanes { get { return enemyPlanes; } }
    public static List<Bullet> Bullets { get { return bullets; } }

    private static List<EnemyPlane> enemyPlanes = new List<EnemyPlane>();
    private static List<Bullet> bullets = new List<Bullet>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CreateBulletPool();
        CreateEnemyPool();
    }

    void CreateBulletPool()
    {
        for (int i = 0; i < PlayerBulletCount; i++)
        {
            GameObject go = Instantiate(PlayerBulletPrefab, PlayerBulletContainer);
            bullets.Add(new PlayerBullet(go.transform));
        }

        for (int i = 0; i < EnemyBulletCount; i++)
        {
            GameObject go = Instantiate(EnemyBulletPrefab, EnemyBulletContainer);
            bullets.Add(new EnemyBullet(go.transform));
        }
    }

    void CreateEnemyPool()
    {
        for (int i = 0; i < enemyPoolInfo.Length; i++)
        {
            for (int j = 0; j < enemyPoolInfo[i].Count; j++)
            {
                GameObject go = Instantiate(enemyPoolInfo[i].Prefab, enemyPoolInfo[i].Container);
                EnemyPlane plane = go.GetComponent<EnemyPlane>();
                plane.alive = false;
                enemyPlanes.Add(plane);
                go.SetActive(false);
            }
        }
    }

    public Bullet GetBullet<T>() where T : Bullet
    {
        Bullet bullet = bullets.Find(x => !x.alive && x is T);

        if (bullet != null) return bullet;
        throw new System.Exception(string.Format("all bullets of type {0} in pool are in alive. consider increasing the item count or destroy unused items", typeof(T)));
    }

    public EnemyPlane GetEnemyPlane(EnemyType type)
    {
        EnemyPlane plane = enemyPlanes.Find(x => !x.alive && x.type == type);

        if (plane != null) return plane;
        throw new System.Exception(string.Format("all plane of type {0} in pool are in alive. consider increasing the item count or destroy unused items", type));
    }

    public void DestroyBullet(Transform transform)
    {
        Bullet bullet = bullets.Find(x => x.transform == transform);

        if (bullet != null)
            bullet.Destroy();
    }

}

[System.Serializable]
public struct EnemyPoolInfo
{
    public int Count;
    public GameObject Prefab;
    public Transform Container;
}