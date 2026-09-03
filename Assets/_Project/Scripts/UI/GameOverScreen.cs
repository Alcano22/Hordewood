using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using TMPro;
using Hordewood.Core;
using Hordewood.Localization;

namespace Hordewood.UI
{
    public class GameOverScreen : UIScreen
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private TextMeshProUGUI waveReachedText;
        [SerializeField] private LocalizedString waveReachedLabel;

        protected override void OnOpened()
        {
            if (waveReachedText != null && waveManager != null)
                waveReachedText.text = LocalizationService.Instance.GetString(waveReachedLabel, waveManager.CurrentWaveNumber);
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
