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
    // wave config
    public List<GameObject> EnemyList = new();
    private List<GameObject> EnemySpawned = new();
    private List<GameObject> SpawnPositions = new();
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

        _timerLeft = 60f;
        _initializeTimer = false;
        SetStateGame(STATE_GAME.INITIALIZING);
        if(enableDebugs) Debug.Log("[GAME_MANAGER] Initializing : " + GetStateGame());
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
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyList[i].GetComponent<Enemy>().enemyObject.GameObject();
        }
    }

    private void Update()
    {
        if (GetWaveState() == WAVE_STATE.GENERATE_NEW_WAVE)
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
