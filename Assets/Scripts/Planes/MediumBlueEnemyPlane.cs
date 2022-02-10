using UnityEngine;

public class MediumBlueEnemyPlane : MediumEnemyPlane
{
    [Tooltip("maximum range of horizontal position when moving at vantage point")]
    public float horizontalMax = 5f;

    private float timer = 0f;

    public override void Move()
    {
        switch (currentState)
        {
            case MediumPlaneState.MoveToVantage:
                {
                    transform.position = Vector3.MoveTowards(transform.position, vantagePosition, moveSpeed * Time.deltaTime);
                    if(transform.position == vantagePosition)
                    {
                        currentState = MediumPlaneState.AtVantage;
                    }
                    break;
                }
            case MediumPlaneState.AtVantage:
                {
                    timer += Time.deltaTime;
                    float x = Mathf.Sin(timer * moveSpeed * 0.1f) * horizontalMax;
                    transform.position = new Vector3(x, transform.position.y, transform.position.z);
                    break;
                }
        }
    }
}
