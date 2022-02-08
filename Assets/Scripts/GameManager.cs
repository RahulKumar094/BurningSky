using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public GameObject PlanePrefab;
    private Plane Player;

    private bool isGamePaused = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GameObject go = Instantiate(PlanePrefab);
        Player = go.GetComponent<Plane>();
        Player.MoveToStart();

        InputDesire.Instance.EnableInput(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputDesire.Instance.IsButtonHeld)
        {
            Player.Move(InputDesire.Instance.TargetPosition);
        }
    }
}
