using UnityEngine;

public class InputDesire : MonoBehaviour
{
    public static InputDesire Instance { get { return instance; } }
    private static InputDesire instance;

    public Vector3 TargetPosition { get { return targetPosition; } }
    public bool IsButtonHeld { get { return isHeld; } }

    private Vector3 targetPosition;
    private bool inputEnabled;    
    private bool isHeld;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputEnabled)
        {
            isHeld = false;
            return;
        }

        isHeld = Input.GetMouseButton(0);
        if (isHeld)
        {
            targetPosition = Camera.main.ViewportToWorldPoint(Input.mousePosition);
        }
    }

    public void EnableInput(bool enable)
    {
        inputEnabled = enable;
    }
}
