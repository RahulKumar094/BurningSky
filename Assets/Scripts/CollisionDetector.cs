using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private ScrollableBackground ScrollableBackground;
    private void Awake()
    {
        ScrollableBackground = FindObjectOfType<ScrollableBackground>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ScrollableBG"))
            ScrollableBackground.SetNextPosition(other.transform);
    }
}
