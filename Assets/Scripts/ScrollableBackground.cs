using UnityEngine;

public class ScrollableBackground : MonoBehaviour
{
    public float Speed;
    public Texture[] BGTextures = new Texture[5];

    private float zInterval = 0;

    void Start()
    {
        int bgCount = transform.childCount;
        float bgHeight = transform.GetChild(0).localScale.y;

        zInterval = bgCount * bgHeight;
        OnLevelChange();
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

    public void OnLevelChange()
    {
        transform.position = Vector3.zero;

        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        Material material = new Material(renderers[0].sharedMaterial);
        material.mainTexture = BGTextures[GameManager.Instance.Level];
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.sharedMaterial = material;
        }
    }
}
