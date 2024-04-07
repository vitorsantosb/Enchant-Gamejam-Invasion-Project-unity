using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class GameState : MonoBehaviour
{
    public enum STATE_GAME
    {
        MAIN_MENU,
        CHANGE_SCENE,
        INITIALIZING,
        
        GAME_FREEZE_WAVE,
        
    }
    public enum WAVE_STATE
    {
        GENERATE_NEW_WAVE,
        WAITING_FINISH_COUNT,
        STOP_SPAWN,
    }
}
