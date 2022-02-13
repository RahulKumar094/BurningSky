using System;
using UnityEngine;

public class InputDesire : MonoBehaviour
{
    public static InputDesire Instance { get { return instance; } }
    private static InputDesire instance;

    public Action<Vector2, float> PointerDragEvent;

    private Vector3 dragBeginPosition;
    private Vector3 targetDirection;
    private bool inputEnabled;
    private bool isHeld;

    private float sensitivity;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        sensitivity = PlayerPrefs.GetFloat(Game.Sensitivity_Key, 0.5f) * 2f;
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
                PointerDragEvent?.Invoke(targetDirection.normalized, sensitivity);
            }
            dragBeginPosition = Input.mousePosition;
        }
    }

    public void EnableInput(bool enable)
    {
        inputEnabled = enable;
    }

}
