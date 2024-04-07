using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class EnumManager : GameState
{
        private STATE_GAME _state; 
        public STATE_GAME GetStateGame() => this._state;
        public void SetStateGame(STATE_GAME currentState) => this._state = currentState;
        
        //wave state
        private WAVE_STATE _waveState;
        public WAVE_STATE GetWaveState() => this._waveState;
        public void SetWaveState(WAVE_STATE currentWaveState) => this._waveState = currentWaveState;

        public virtual void OnConnectedToMaster()
        {
                throw new System.NotImplementedException();
        }

        public virtual void OnJoinedLobby()
        {
                throw new System.NotImplementedException();
        }
}
