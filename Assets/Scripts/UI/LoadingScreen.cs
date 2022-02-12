using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Transform Circle;
    public float RotateSpeed = 20f;

    private float timer;
    void Start()
    {
        timer = 0f;

        if (!string.IsNullOrEmpty(Game.NextSceneToLoad))
            SceneManager.LoadSceneAsync(Game.NextSceneToLoad);
        else
            throw new System.Exception("next level to load is not set");
    }

    // Update is called once per frame
    void Update()
    {
        RotateCirle();
    }

    private void RotateCirle()
    {
        timer += Time.deltaTime;
        Circle.transform.rotation = Quaternion.Euler(0f, 0f, timer * RotateSpeed);
    }
}
