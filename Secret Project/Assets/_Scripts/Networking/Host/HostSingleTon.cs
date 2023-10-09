using UnityEngine;

namespace _Scripts.Networking.Host
{
    public class HostSingleTon : MonoBehaviour
    {
        private static HostSingleTon _instance;

        public static HostSingleTon Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<HostSingleTon>();

                if (_instance == null)
                {
                    return null;
                }

                return _instance;
            }
        }
        public HostGameManager GameManager { get; set; }

        public void CreateHost()
        {
            GameManager = new HostGameManager();
        }
    }
}