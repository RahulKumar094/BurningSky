using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get { return instance; } }
    private static ObjectPool instance;

    public GameObject PlayerBulletPrefab;
    public GameObject EnemyBulletPrefab;
    public Transform PlayerBulletContainer;
    public Transform EnemyBulletContainer;

    public static int PLAYERBULLETCOUNT = 100;
    public static int ENEMYBULLETCOUNT = 50;

    private List<PlayerBullet> playerBullets = new List<PlayerBullet>();
    private List<EnemyBullet> enemyBullets = new List<EnemyBullet>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CreatePool();
    }

    void CreatePool()
    {
        for (int i = 0; i < PLAYERBULLETCOUNT; i++)
        {
            GameObject go = Instantiate(PlayerBulletPrefab, PlayerBulletContainer);
            playerBullets.Add(new PlayerBullet(go.transform));
        }

        for (int i = 0; i < ENEMYBULLETCOUNT; i++)
        {
            GameObject go = Instantiate(EnemyBulletPrefab, EnemyBulletContainer);
            enemyBullets.Add(new EnemyBullet(go.transform));
        }
    }

    public Bullet GetBullet<T>() where T : Bullet
    {
        Bullet bullet = null;
        if (typeof(T) == typeof(PlayerBullet))
            bullet = playerBullets.Find(x => !x.alive);
        else if(typeof(T) == typeof(EnemyBullet))
            bullet = enemyBullets.Find(x => !x.alive);
        else
            throw new System.Exception("entry must be a type of concrete class");

        if (bullet != null) return bullet;
        throw new System.Exception("all items in pool are in alive. consider increasing the item count or destroy unused items");
    }
}
