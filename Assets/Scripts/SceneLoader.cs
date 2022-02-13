using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void LoadGameLevel()
    {
        if (string.Compare(SceneManager.GetActiveScene().name, SceneNames.MainMenu) == 0)
        {
            SceneManager.LoadSceneAsync(0);
        }
        else if(Game.Level > 1)
        {
            SceneManager.UnloadSceneAsync(Game.Level - 1);
        }
        SceneManager.LoadSceneAsync(Game.Level, LoadSceneMode.Additive);
    }

    public static void LoadMainMenuScene()
    {
        SceneManager.LoadSceneAsync(SceneNames.MainMenu);
    }
}

public class SceneNames
{
    public const string Loading = "Loading";
    public const string MainMenu = "MainMenu";
    public const string Level_0 = "Level_0";
    public const string Level_1 = "Level_1";
    public const string Level_2 = "Level_2";
}
