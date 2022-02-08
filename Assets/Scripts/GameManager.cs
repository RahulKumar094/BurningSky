using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public GameObject PlanePrefab;

    private List<Bullet> aliveBulletList = new List<Bullet>();

    private Plane player;
    private bool isGamePaused = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GameObject go = Instantiate(PlanePrefab);
        player = go.GetComponent<Plane>();
        player.SetAttribute(500f, 30f, 5f);
        player.MoveToStart();

        InputDesire.Instance.EnableInput(true);
        InputDesire.Instance.PointerDragEvent += player.Move;
    }

    void Update()
    {
        foreach (Bullet bullet in aliveBulletList)
        {
            bullet.Update();
        }
    }

    public void ShootBullet(Vector3 startPosition, Vector3 direction)
    {
        Bullet bullet = ObjectPool.Instance.GetBullet<PlayerBullet>();
        bullet.Shoot(startPosition, direction);
        aliveBulletList.Add(bullet);
    }

}
