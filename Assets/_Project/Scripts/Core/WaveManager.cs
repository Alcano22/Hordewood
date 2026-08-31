using UnityEngine;
using System;
using System.Collections;
using Hordewood.Enemies;

namespace Hordewood.Core
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private WaveData[] waves;
        [SerializeField] private float delayBetweenWaves = 3f;

        private int _currentWaveIndex = -1;
        private float _waveTimer;
        private bool _waveActive;

        public WaveData CurrentWave => _currentWaveIndex >= 0 && _currentWaveIndex < waves.Length
                                     ? waves[_currentWaveIndex] : null;

        public event Action<WaveData> OnWaveStarted;
        public event Action OnAllWavesComplete;

        private void Start() => StartNextWave();

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.Playing) return;
            if (!_waveActive || CurrentWave == null) return;

            _waveTimer -= Time.deltaTime;
            if (_waveTimer <= 0f)
                EndWave();
        }

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
            StartCoroutine(WaitAndStartNextWave());
        }

        private void KillAllEnemies()
        {
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
                enemy.Kill();
        }

        private IEnumerator WaitAndStartNextWave()
        {
            yield return new WaitForSeconds(delayBetweenWaves);
            StartNextWave();
        }

        public float TimeRemaining => _waveTimer;
        public bool IsWaveActive => _waveActive;
    }
}
