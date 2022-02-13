
public class Game
{
    public const int CoinToScoreMultiplier = 50;
    public const string Highscore_Key = "High_Score_Key";
    public const string Sensitivity_Key = "Setting_Sensitivity_Key";

    public static int Level = 1;
    public const int LevelMax = 5;
    public static int[] LevelHighscore = new int[LevelMax];
}

public class Tags
{
    public const string ScrollableBackgroundTag = "ScrollableBG";
    public const string InvinsibilityWall = "InvinsibilityWall";
    public const string PlayerBulletTag = "PlayerBullet";
    public const string EnemyBulletTag = "EnemyBullet";
    public const string HomingMissile = "Missile";
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string CoinTag = "Coin";
}
