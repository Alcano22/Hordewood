using UnityEngine;
using UnityEngine.InputSystem;
using Hordewood.Enemies;
using Hordewood.Input;

namespace Hordewood.Core
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private WaveGenerationConfig generationConfig;

        private PlayerControls _controls;
        private WaveData _currentWave;
        private int _currentWaveIndex;
        private float _waveTimer;
        private bool _waveActive;
        private bool _waitingForReady;

        public event System.Action<WaveData> OnWaveStarted;
        public event System.Action OnWaveEnded;

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
            _currentWave = generationConfig.GenerateWave(_currentWaveIndex);
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

        public WaveData CurrentWave => _currentWave;
        public int CurrentWaveNumber => _currentWaveIndex;
        public float TimeRemaining => _waveTimer;
        public bool IsWaveActive => _waveActive;
        public bool IsWaitingForReady => _waitingForReady;
    }
}
