
public class Game
{
    //must be set before loading scene is loaded
    public static string NextSceneToLoad;
}

public class SceneNames
{
    public const string Loading = "Loading";
    public const string MainMenu = "MainMenu";
    public const string Level_1 = "Level_1";
    public const string Level_2 = "Level_2";
    public const string Level_3 = "Level_3";
    public const string Level_4 = "Level_4";
    public const string Level_5 = "Level_5";
}

public class Tags
{
    public const string InvinsibilityWall = "InvinsibilityWall";
    public const string PlayerBulletTag = "PlayerBullet";
    public const string EnemyBulletTag = "EnemyBullet";
    public const string HomingMissile = "Missile";
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string CoinTag = "Coin";
}
