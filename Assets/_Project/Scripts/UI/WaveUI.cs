using UnityEngine;
using UnityEngine.Localization;
using TMPro;
using Hordewood.Core;
using Hordewood.Localization;

namespace Hordewood.UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private LocalizedString waveLabel;
        [SerializeField] private LocalizedString readyPrompt;

        private string _displayedWaveName;

        private void Update()
        {
            if (waveManager.IsWaitingForReady)
            {
                waveText.text = LocalizationService.Instance.GetString(readyPrompt);
                return;
            }

            if (waveManager.CurrentWave == null) return;

            float time = Mathf.Max(0f, waveManager.TimeRemaining);
            int mins = Mathf.FloorToInt(time / 60f);
            int secs = Mathf.FloorToInt(time % 60f);

            string waveName = LocalizationService.Instance.GetString(waveLabel, waveManager.CurrentWaveNumber);
            waveText.text = $"{waveName} — {mins}:{secs:00}";
        }
    }
}
