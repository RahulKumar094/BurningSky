using UnityEngine;

public class MediumGreyEnemyPlane : MediumEnemyPlane
{
    public float criticalAtHealth = 200f;
    public float suicideSpeedMultiplier = 5f;

    public override void Move()
    {
        switch (currentState)
        {
            case MediumPlaneState.MoveToVantage:
                {
                    transform.position = Vector3.MoveTowards(transform.position, vantagePosition, moveSpeed * Time.deltaTime);
                    if (transform.position == vantagePosition)
                    {
                        currentState = MediumPlaneState.AtVantage;
                    }
                    break;
                }
            case MediumPlaneState.AtVantage:
                {
                    if (health <= criticalAtHealth)
                    {
                        currentState = MediumPlaneState.SuicideBomb;
                    }
                    break;
                }
            case MediumPlaneState.SuicideBomb:
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, moveSpeed * suicideSpeedMultiplier * Time.deltaTime);
                    break;
                }
        }

    }
}