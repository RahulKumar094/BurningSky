using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemyPlane : EnemyPlane
{
    private enum State
    {
        MoveToVantage,
        AtVantage,
        SuicideBomb
    }

    public Transform Weapon_Left;
    public Transform Weapon_Right;

    //for blue enemy plane
    private float timer = 0f;
    private const float xMax = 5f;

    //for grey enemy plane
    private float criticalHealth = 200f;

    private State currentState;
    private Vector3 vantagePosition = new Vector3(0f, -20f, 10f);
    

    public override void SpawnAt(Vector3 spawnPosition)
    {
        base.SpawnAt(spawnPosition);

        currentState = State.MoveToVantage;
        StartCoroutine("Shoot");
    }

    public override void Move()
    {
        switch (currentState)
        {
            case State.MoveToVantage:
                {
                    transform.position = Vector3.MoveTowards(transform.position, vantagePosition, moveSpeed * Time.deltaTime);
                    if(transform.position == vantagePosition)
                    {
                        currentState = State.AtVantage;
                    }
                    break;
                }
            case State.AtVantage:
                {
                    if (type == EnemyType.MediumBlue)
                    {
                        timer += Time.deltaTime;
                        float x = Mathf.Sin(timer * moveSpeed * 0.1f) * xMax;
                        transform.position = new Vector3(x, transform.position.y, transform.position.z);
                    }
                    else if (type == EnemyType.MediumGrey && health <= criticalHealth) 
                    {
                        currentState = State.SuicideBomb;
                    }
                    break;
                }
            case State.SuicideBomb:
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, moveSpeed * 5f * Time.deltaTime);
                    break;
                }
        }
        
    }

    public override void Update()
    {
        if (alive)
        {
            Move();
        }
    }

    public override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);
        while (alive)
        {
            GameManager.Instance.ShootEnemyBullet(Weapon_Left.position, Weapon_Left.forward);
            GameManager.Instance.ShootEnemyBullet(Weapon_Right.position, Weapon_Right.forward);
            yield return new WaitForSeconds(1 / fireRate);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag(Tags.PlayerBulletTag))
        {
            GameManager.Instance.EnemyCollideWithBullet(other.transform, this);
        }
    }
}
