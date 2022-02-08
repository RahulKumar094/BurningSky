public interface IAttribute
{
    public float health { set; get; }
    public float moveSpeed { set; get; }
    public float fireRate { set; get; }

    public void SetAttribute(float health, float moveSpeed, float fireRate);
}