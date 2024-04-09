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
    public GameObject playerObj;


    // wave config
    public List<GameObject> EnemyList = new();
    [SerializeField] private List<GameObject> EnemySpawned = new();
    public List<GameObject> SpawnPositions = new();

    private int _waveCount;
    private int _waveValue;
    private int _turnCount;
    private int _waveMultiply;
    private float _waveSpawnCooldown;
    private bool _enableSpawn;
    private int _waveSpawnLimit;
    private int _enemyCountSpawned;

    //Timer Interface config
    public Text _timerTxt;
    private float _timerLeft;
    private bool _initializeTimer;

    //Interface buttons
    public Button readyButton;

    //Interface UI
    public Text _turnCountTxt;

    public GameObject deathScreenGameObject;

    public GameObject winScreenGameObject;

    //console debugs 
    [SerializeField] private bool enableDebugs;

    public void Start()
    {
        this._timerLeft = gameManagerConfig.timerInSeconds;
        this._waveCount = gameManagerConfig.waveInitialCount;
        this._waveValue = gameManagerConfig.waveValue;
        this.enableDebugs = gameManagerConfig.enableDebugs;
        this._waveMultiply = gameManagerConfig.waveMultiplyCount;
        this._waveSpawnCooldown = gameManagerConfig.spawnCooldown;
        this._waveSpawnLimit = gameManagerConfig.waveSpawnLimit;

        this._enableSpawn = true;

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
        if (enableDebugs) Debug.Log("[GAME_MANAGER] STATE_GAME: " + GetStateGame());
        // Verificar se é possível spawnar inimigos
        if (!_enableSpawn) return;

        int enemiesToSpawn = Mathf.Min(_waveValue, _waveSpawnLimit - EnemySpawned.Count);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomSpawn = Random.Range(0, SpawnPositions.Count);
            if (enableDebugs) Debug.Log("RandomNumber for spawn " + randomSpawn);

            Vector3 _spawnPos = SpawnPositions[randomSpawn].transform.position;
            GameObject enemyObj = EnemyList[Random.Range(0, EnemyList.Count)];

            Instantiate(enemyObj, _spawnPos, Quaternion.identity);
            EnemySpawned.Add(enemyObj);
        }

        // Verificar se atingiu o limite de spawn
        if (EnemySpawned.Count >= _waveSpawnLimit)
        {
            Debug.Log("[Spawn Count]: " + EnemySpawned.Count);
            SetWaveState(WAVE_STATE.STOP_SPAWN);
            _enableSpawn = false;
        }
    }

    private void WaveCooldownSpawn()
    {
        if (GetWaveState() == WAVE_STATE.GENERATE_NEW_WAVE && !_enableSpawn)
        {
            if (_waveSpawnCooldown > 0)
            {
                _waveSpawnCooldown -= Time.deltaTime;
            }
            else
            {
                _enableSpawn = true;
                _waveSpawnCooldown = gameManagerConfig.spawnCooldown; // Reset cooldown
            }
        }
    }

    private void Update()
    {
        ReadyToStartGame();
        if (GetStateGame() == STATE_GAME.START_TURN)
        {
            Debug.Log("[GAME_MANAGER] STATE_GAME: " + GetStateGame());
            if (_timerLeft > 0)
            {
                _timerLeft -= Time.deltaTime;
                UpdateTimer(_timerLeft);
                WaveCooldownSpawn();
            }
            else
            {
                SetStateGame(STATE_GAME.END_TURN);
                UpdateGameVarieblesForNextTurn();
                if (enableDebugs) Debug.Log("[GAME_MANAGER]: " + GetWaveState());
                _timerLeft = 0;
                _initializeTimer = false;
            }
        }

        if (GetWaveState() == WAVE_STATE.GENERATE_NEW_WAVE && EnemySpawned.Count >= _waveSpawnLimit)
        {
            SetWaveState(WAVE_STATE.STOP_SPAWN);
            _enableSpawn = false;
        }
    }

    void CountDownToStart()
    {
        if (GetStateGame() == STATE_GAME.COUNTDOWN_TO_START)
        {
            SetStateGame(STATE_GAME.START_TURN);
            SetWaveState(WAVE_STATE.GENERATE_NEW_WAVE);
            readyButton.gameObject.SetActive(false);
            GenerateWave();
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
        this._waveSpawnLimit = _waveSpawnLimit + 10;
        this._turnCount += 1;
        this._turnCountTxt.text = _turnCount.ToString();
        this._timerLeft = gameManagerConfig.timerInSeconds;

        if (_turnCount == 10)
        {
            if (winScreenGameObject != null)
            {
                GameObject defaultScreen = GameObject.FindGameObjectWithTag("MainCanvas");
                defaultScreen.SetActive(false);
                winScreenGameObject.SetActive(true);
            }
            //TODO finish the game
        }

        if (EnemySpawned.Count > 0)
        {
            for (int i = 0; i <= EnemySpawned.Count; i++)
            {
                Destroy(this.EnemySpawned[i].gameObject);
                if (enableDebugs) Debug.Log("[DESTROYED] GameObject: " + EnemySpawned[i].gameObject.name);
                EnemySpawned.Remove(EnemySpawned[i]);
            }

            EnemySpawned.Clear();
            if (enableDebugs) Debug.Log("[GAME_MANAGER] Enemy List Cleared: " + EnemySpawned.Count);
        }

        SetWaveState(WAVE_STATE.GENERATE_NEW_WAVE);
        SetStateGame(STATE_GAME.START_TURN);
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