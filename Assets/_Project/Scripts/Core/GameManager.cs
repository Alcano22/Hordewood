using UnityEngine;
using System;

namespace Hordewood.Core
{
    public enum GameState { Playing, Paused, GameOver }

    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentState { get; private set; } = GameState.Playing;

        public event Action<GameState> OnStateChanged;
        
        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            Time.timeScale = newState == GameState.Playing ? 1f : 0f;
            OnStateChanged?.Invoke(newState);
        }
    }
}
