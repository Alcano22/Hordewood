using UnityEngine;

namespace Hordewood.Core
{
    public class GameSeed : MonoBehaviour
    {
        public static int CurrentSeed { get; private set; }

        [SerializeField] private bool useFixedSeed = false;
        [SerializeField] private int fixedSeed = 0;

        private void Awake()
        {
            CurrentSeed = useFixedSeed ? fixedSeed : System.Environment.TickCount;
            Random.InitState(CurrentSeed);

            Debug.Log($"World Seed: {CurrentSeed}");
        }
    }
}
