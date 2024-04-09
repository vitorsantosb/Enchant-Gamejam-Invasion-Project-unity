using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
public class GameManager : EnumManager
{
    //Config ScriptableObject
    public GameManagerScriptableObject gameManagerConfig;

    // spawn map props (items, etc...)
    public SpawnMapProps spawnMapProps;

    // wave config
    public List<GameObject> EnemyList = new();
    [SerializeField] private List<GameObject> EnemySpawned = new();
    public List<GameObject> SpawnPositions = new();

    private int _waveCount;
    private int _waveValue;
    private int _turnCount;
    private int _waveMultiply;

    //Timer Interface config
    public Text _timerTxt;
    private float _timerLeft;
    private bool _initializeTimer;
    

    //Interface buttons
    public Button readyButton;

    //Interface UI
    
    //console debugs 
    [SerializeField] private bool enableDebugs;

    public void Start()
    {
        this._timerLeft = gameManagerConfig.timerInSeconds;
        this._waveCount = gameManagerConfig.waveInitialCount;
        this._waveValue = gameManagerConfig.waveValue;
        this.enableDebugs = gameManagerConfig.enableDebugs;
        this._waveMultiply = gameManagerConfig.waveMultiplyCount;
        
        SetStateGame(STATE_GAME.INITIALIZING);
        if (enableDebugs) Debug.Log("[GAME_MANAGER] Initializing : " + GetStateGame());
    }

    public void GenerateWave()
    {
        if (GetWaveState() == WAVE_STATE.GENERATE_NEW_WAVE)
        {
            GenerateEnemies();
        }
    }

    private void GenerateEnemies()
    {
        if (enableDebugs) Debug.Log("[GAME_MANAGER] Spawning enemies");
        for (int i = 0; i < _waveValue; i++)
        {
            int randomSpawn = Random.Range(0, SpawnPositions.Count + 1);
            if (enableDebugs) Debug.Log("RandomNumber for spawn " + randomSpawn);

            Vector3 _spawnPos = SpawnPositions[randomSpawn].GetComponent<Transform>().position;
            GameObject enemyObj = EnemyList[i].GameObject();

            Instantiate(enemyObj, _spawnPos, Quaternion.identity);

            EnemySpawned.Add(enemyObj);
        }
    }

    private void Update()
    {
        ReadyToStartGame();
        if (GetStateGame() == STATE_GAME.START_TURN)
        {
            if (_timerLeft > 0)
            {
                _timerLeft -= Time.deltaTime;
                UpdateTimer(_timerLeft);
            }
            else
            {
                SetWaveState(WAVE_STATE.STOP_SPAWN);

                if (enableDebugs) Debug.Log("[GAME_MANAGER]: " + GetWaveState());
                _timerLeft = 0;
                _initializeTimer = false;
            }
        }
    }

    void CountDownToStart()
    {
        if (GetStateGame() == STATE_GAME.COUNTDOWN_TO_START)
        {
            SetWaveState(WAVE_STATE.GENERATE_NEW_WAVE);
            readyButton.gameObject.SetActive(false);
            GenerateEnemies();

            _turnCount++;
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        _timerTxt.text = $"{minutes:00} : {seconds:00}";
    }

    void UpdateGameVarieblesForNextTurn()
    {
        this._waveValue = _waveValue * 2;
    }

    void ReadyToStartGame()
    {
        if (readyButton.GetComponent<StartButton>().isReady)
        {
            SetStateGame(STATE_GAME.COUNTDOWN_TO_START);
            CountDownToStart();
        }
    }
}