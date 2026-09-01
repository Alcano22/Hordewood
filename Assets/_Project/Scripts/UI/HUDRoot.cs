using UnityEngine;

namespace Hordewood.UI
{
    public class HUDRoot : MonoBehaviour
    {
        [SerializeField] private GameObject hudContent;

        private void Start()
        {
            if (ScreenManager.Instance != null)
                ScreenManager.Instance.OnAnyScreenOpenChanged += SetHUDVisible;
        }

        private void OnDestroy()
        {
            if (ScreenManager.Instance != null)
                ScreenManager.Instance.OnAnyScreenOpenChanged -= SetHUDVisible;
        }

        private void SetHUDVisible(bool anyScreenOpen) => hudContent.SetActive(!anyScreenOpen);
    }
}
