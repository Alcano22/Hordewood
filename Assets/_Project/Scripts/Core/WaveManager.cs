using UnityEngine;
using Hordewood.Enemies;
using Hordewood.Input;
using UnityEngine.InputSystem;

namespace Hordewood.Core
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private WaveData[] waves;

        private PlayerControls _controls;
        private int _currentWaveIndex = -1;
        private float _waveTimer;
        private bool _waveActive;
        private bool _waitingForReady;

        public WaveData CurrentWave => _currentWaveIndex >= 0 && _currentWaveIndex < waves.Length
                                     ? waves[_currentWaveIndex] : null;

        public event System.Action<WaveData> OnWaveStarted;
        public event System.Action OnWaveEnded;
        public event System.Action OnAllWavesComplete;

        private void OnEnable()
        {
            _controls = PlayerControlsProvider.Instance.Controls;
            _controls.Player.Ready.performed += OnReadyPressed;
        }

        private void OnDisable()
        {
            _controls.Player.Ready.performed -= OnReadyPressed;
        }

        private void Start() => StartNextWave();

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.Playing) return;
            if (!_waveActive || CurrentWave == null) return;

            _waveTimer -= Time.deltaTime;
            if (_waveTimer <= 0f)
                EndWave();
        }

        private void OnReadyPressed(InputAction.CallbackContext ctx) => NotifyPlayerReady();

        private void StartNextWave()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex >= waves.Length)
            {
                OnAllWavesComplete?.Invoke();
                return;
            }

            _waveTimer = CurrentWave.Duration;
            _waveActive = true;
            OnWaveStarted?.Invoke(CurrentWave);
        }

        private void EndWave()
        {
            _waveActive = false;
            KillAllEnemies();

            _waitingForReady = true;
            GameManager.Instance.SetState(GameState.WaveBreak);
            OnWaveEnded?.Invoke();
        }

        public void NotifyPlayerReady()
        {
            if (!_waitingForReady) return;

            _waitingForReady = false;
            GameManager.Instance.SetState(GameState.Playing);
            StartNextWave();
        }

        private void KillAllEnemies()
        {
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
                enemy.Kill();
        }

        public float TimeRemaining => _waveTimer;
        public bool IsWaveActive => _waveActive;
        public bool IsWaitingForReady => _waitingForReady;
    }
}
