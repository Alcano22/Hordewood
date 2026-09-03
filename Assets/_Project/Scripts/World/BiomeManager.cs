using UnityEngine;
using Hordewood.Core;

namespace Hordewood.World
{
    public class BiomeManager : MonoBehaviour
    {
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private BiomeData[] biomes;
        [SerializeField] private int wavesPerBiome = 10;

        private int _lastSlot = -1;

        public BiomeData CurrentBiome { get; private set; }
        public BiomeData UpcomingBiome { get; private set; }

        public event System.Action<BiomeData> OnBiomeChanged;

        private void Awake()
        {
            CurrentBiome = biomes[0];
            UpcomingBiome = RollNextBiome(CurrentBiome);
        }

        private void OnEnable() => waveManager.OnWaveStarted += OnWaveStarted;
        private void OnDisable() => waveManager.OnWaveStarted -= OnWaveStarted;

        private void OnWaveStarted(WaveData wave)
        {
            int slot = (waveManager.CurrentWaveNumber - 1) / wavesPerBiome;
            if (slot == _lastSlot) return;
            _lastSlot = slot;

            if (slot == 0) return;

            CurrentBiome = UpcomingBiome;
            UpcomingBiome = RollNextBiome(CurrentBiome);
            OnBiomeChanged?.Invoke(CurrentBiome);
        }

        private BiomeData RollNextBiome(BiomeData exclude)
        {
            if (biomes.Length == 1)
                return biomes[0];

            BiomeData result;
            do
            {
                result = biomes[Random.Range(0, biomes.Length)];
            } while (result == exclude);

            return result;
        }
    }
}
