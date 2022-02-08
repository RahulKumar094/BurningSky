using UnityEngine;

public class ScrollableBackground : MonoBehaviour
{
    public float Speed;

    private float zInterval = 0;

    void Start()
    {
        int bgCount = transform.childCount;
        float bgHeight = transform.GetChild(0).localScale.y;

        zInterval = bgCount * bgHeight;
    }

    void Update()
    {
        //scroll bg
        transform.position -= transform.forward * Speed * Time.deltaTime;
    }

    public void SetNextPosition(Transform transform)
    {
        Vector3 oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z + zInterval);

        //invert scale to remove seam
        Vector3 oldScale = transform.localScale;
        transform.localScale = new Vector3(oldScale.x, -oldScale.y, oldScale.z);
    }
}
