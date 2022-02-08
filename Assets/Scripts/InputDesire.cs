using System;
using UnityEngine;

public class InputDesire : MonoBehaviour
{
    public static InputDesire Instance { get { return instance; } }
    private static InputDesire instance;

    public Action<Vector2> PointerDragEvent;

    private Vector3 dragBeginPosition;
    private Vector3 targetDirection;
    private bool inputEnabled;
    private bool isHeld;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!inputEnabled)
        {
            isHeld = false;
            return;
        }

        //first touch
        if (!isHeld && Input.GetMouseButton(0))
        {
            dragBeginPosition = Input.mousePosition;
        }

        isHeld = Input.GetMouseButton(0);
        if (isHeld)
        {
            targetDirection = Input.mousePosition - dragBeginPosition;
            if (targetDirection.magnitude != 0)
            {
                PointerDragEvent?.Invoke(targetDirection.normalized);
            }
            dragBeginPosition = Input.mousePosition;
        }
    }

    public void EnableInput(bool enable)
    {
        inputEnabled = enable;
    }

}
