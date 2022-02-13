
using UnityEngine;

public class Game
{
    public const string Highscore_Key = "High_Score_Key";
    public const string Sensitivity_Key = "Setting_Sensitivity_Key";

    public static int Level = 1;
    public const int LevelMax = 5;
    public const int CoinToScoreMultiplier = 50;

    public static readonly MinMax XAxisSpawnRange = new MinMax(-7f, 7f);
    public static readonly Vector3 EnemySpawnPosition = new Vector3(0, -20f, 22f);
    public static readonly Vector3 PlayerSpawnPosition = new Vector3(0, -20, -8);

    //player movement bound
    public static readonly MinMax XBound = new MinMax(-8.8f, 8.8f);
    public static readonly MinMax ZBound = new MinMax(-15.2f, 15.2f);
}

public class DamageData
{
    public const int FromPlayerBullet = 100;
    public const int FromPlayerPlane = 200;
    public const int FromPlayerMissile = 400;

    public const int FromEnemyBullet = 50;    
    public const int FromEnemyPlane = 150;
    public const int FromEnemyMissile = 200;
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
