using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float Speed = 50f;

    private readonly Vector3 StartPosition = new Vector3(0, -20, -8);

    private bool canMove;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToStart()
    {
        StartCoroutine("MoveToStartCoroutine");
    }

    private IEnumerator MoveToStartCoroutine()
    {
        canMove = false;

        while (transform.position != StartPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPosition, Speed * Time.deltaTime);
            yield return null;
        }

        canMove = true;
    }

    public void Move(Vector3 targetPosition)
    {
        Debug.LogError(transform.position + "_" + targetPosition);
        if(canMove)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }
}
