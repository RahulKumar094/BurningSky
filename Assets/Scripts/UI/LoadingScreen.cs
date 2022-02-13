using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Transform Circle;
    public float RotateSpeed = 20f;

    private float timer;
    //void Start()
    //{
    //    timer = 0f;
    //    AsyncOperation handle = SceneManager.LoadSceneAsync(SceneNames.Level);
    //    handle.completed += LevelLoaded;
    //}

    //void LevelLoaded(AsyncOperation handle)
    //{
    //    SceneManager.LoadSceneAsync(SceneLoader.NextSceneToLoad, LoadSceneMode.Additive);
    //}

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
