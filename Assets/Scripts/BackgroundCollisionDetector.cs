using UnityEngine;

public class BackgroundCollisionDetector : MonoBehaviour
{
    private ScrollableBackground ScrollableBackground;
    private const string ScrollableBackgroundTag = "ScrollableBG";

    private void Awake()
    {
        ScrollableBackground = FindObjectOfType<ScrollableBackground>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ScrollableBackgroundTag))
            ScrollableBackground.SetNextPosition(other.transform);
    }
}
