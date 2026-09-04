using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hordewood.UI
{
    public class MainMenuScreen : UIScreen
    {
        [SerializeField] private string gameSceneName = "Game";

        protected override void Start()
        {
            base.Start();
            Open();
        }

        public void PlayGame() => SceneManager.LoadScene(gameSceneName);
        public void OpenSettings() => ScreenManager.Instance.Open<SettingsScreen>();
        public void OpenCredits() => ScreenManager.Instance.Open<CreditsScreen>();
        public void QuitGame() => Application.Quit();
    }
}
