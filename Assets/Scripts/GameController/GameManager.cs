using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
public class GameManager : EnumManager
{
    // spawn map props (items, etc...)
    public SpawnMapProps spawnMapProps;

    // wave config
    public List<GameObject> EnemyList = new();
    private List<GameObject> EnemySpawned = new();
    
    private int _waveCount;
    private int _waveValue;
    
    //Timer config
    public Text _timerTxt;
    private float _timerLeft;
    private bool _initializeTimer;
    
    //console debugs 
    [SerializeField] private bool enableDebugs = true;
    public void Start()
    {
        SetStateGame(STATE_GAME.INITIALIZING);
        if(enableDebugs) Debug.Log("[GAME_MANAGER] Waiting Photon server connection: " + GetStateGame());
    }

    public void GenerateWave()
    {
        if (GetWaveState() == WAVE_STATE.GENERATE_NEW_WAVE)
        {
            _waveValue = _waveCount * 10;
        }
    }

    public void GenerateEnemies()
    {
        
    }

    private void Update()
    {
        if (_initializeTimer)
        {
            if (_timerLeft > 0)
            {
                _timerLeft -= Time.deltaTime;
                UpdateTimer(_timerLeft);
            }
            else
            {
                _timerLeft = 0;
                _initializeTimer = false;
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        _timerTxt.text = $"{minutes:00} : {seconds:00}";
    }
}
