using UnityEngine;
using TMPro;
using Hordewood.Core;

namespace Hordewood.UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private TextMeshProUGUI waveText;

        private string _displayedWaveName;

        private void OnEnable()
        {
            waveManager.OnWaveStarted += OnWaveStarted;
        }

        private void OnDisable()
        {
            waveManager.OnWaveStarted -= OnWaveStarted;
        }

        private void OnWaveStarted(WaveData wave)
        {
            _displayedWaveName = wave.DisplayName;
        }

        private void Update()
        {
            if (waveManager.CurrentWave == null) return;

            float time = Mathf.Max(0f, waveManager.TimeRemaining);
            int mins = Mathf.FloorToInt(time / 60f);
            int secs = Mathf.FloorToInt(time % 60f);
            waveText.text = $"{_displayedWaveName} — {mins}:{secs:00}";
        }
    }
}
