using UnityEngine;

[CreateAssetMenu]
public class GameDataSO : ScriptableObject
{
    public bool bAIEnabled = false;
    public bool bAIIsWhite = false;
    public int AIDifficulty = 0;
}
