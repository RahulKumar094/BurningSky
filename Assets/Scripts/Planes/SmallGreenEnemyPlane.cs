using System.Collections;
using UnityEngine;

public class SmallGreenEnemyPlane : SmallEnemyPlane
{
    public float rotationSpeed = 50f;
    public float yRotationAngle = 90f;

    public void SetRotation(float yAxisMultiplier, float delay)
    {
        StartCoroutine(RotateAtAngle(yAxisMultiplier * yRotationAngle, rotationSpeed, delay));
    }

    private IEnumerator RotateAtAngle(float yAxisRotation, float rotationSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation = new Vector3(eulerRotation.x, eulerRotation.y + yAxisRotation, eulerRotation.z);
        while (alive)
        {
            Vector3 rotation = Vector3.MoveTowards(transform.rotation.eulerAngles, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotation);
            yield return null;
        }
    }

}
