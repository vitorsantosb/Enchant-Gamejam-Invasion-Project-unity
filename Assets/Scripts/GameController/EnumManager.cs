using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class EnumManager : GameState
{ 
        private State_Game _state; 
        public State_Game GetStateGame() => this._state;
        public void SetStateGame(State_Game currentState) => this._state = currentState;
}
