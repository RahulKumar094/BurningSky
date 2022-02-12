using UnityEngine;

[CreateAssetMenu(menuName = "Coin Value")]
public class Enemy_CoinValue : ScriptableObject
{
    public CoinValue[] coinValue = new CoinValue[3];
}

[System.Serializable]
public struct CoinValue
{
    public CoinType type;
    public int count;
}
