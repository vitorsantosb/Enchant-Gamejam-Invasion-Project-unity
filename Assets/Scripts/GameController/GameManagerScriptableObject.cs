using UnityEngine;

[CreateAssetMenu(fileName = "gameManagerConfig", menuName = "GameManager/New GameManager File", order = 0)]
public class GameManagerScriptableObject : ScriptableObject
{
    [Header("Wave Atributes")] 
    public int waveValue;
    public int waveInitialCount;
    public int waveIncrementByRound;
    public int waveMultiplyCount;
    public int spawnCooldown;

    [Header("Timer Config")] 
    public float timerInSeconds;

    [Header("System Config")] 
    public bool enableDebugs;
}