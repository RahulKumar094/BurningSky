using UnityEngine;

public class BackgroundCollisionDetector : MonoBehaviour
{
    private ScrollableBackground ScrollableBackground;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.ScrollableBackgroundTag))
        {
            if (ScrollableBackground == null)
            {
                ScrollableBackground = FindObjectOfType<ScrollableBackground>();
            }
            ScrollableBackground.SetNextPosition(other.transform);
        }
    }
}
