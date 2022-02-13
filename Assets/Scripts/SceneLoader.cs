using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void LoadGameLevelScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync(Game.Level, LoadSceneMode.Additive).completed += (handle) => 
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(Game.Level));
        };
    }

    public static void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(SceneNames.MainLevelScene);
        SceneManager.LoadSceneAsync(Game.Level, LoadSceneMode.Additive).completed += (handle) =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(Game.Level));
        };
    }

    public static void LoadMainMenuScene()
    {
        SceneManager.LoadSceneAsync(SceneNames.MainMenu);
    }
}

public class SceneNames
{
    public const string MainLevelScene = "Level_Main";
    public const string MainMenu = "MainMenu";
}
