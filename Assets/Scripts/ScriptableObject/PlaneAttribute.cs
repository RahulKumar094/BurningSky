using UnityEngine;

[CreateAssetMenu(menuName = "Plane Attribute")]
public class PlaneAttribute : ScriptableObject
{
    public float health;
    public float moveSpeed;

    [Tooltip("rounds fired per second")]
    public float fireRate;
}


public interface IAttribute
{
    public bool alive { set; get; }
    public float health { set; get; }
    public float moveSpeed { set; get; }
    public float fireRate { set; get; }

    public void SetAttribute(PlaneAttribute attribute);
}