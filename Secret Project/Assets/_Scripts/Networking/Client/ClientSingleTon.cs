using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Networking.Client
{
    public class ClientSingleTon : MonoBehaviour
    {
        private static ClientSingleTon _instance;

        public static ClientSingleTon Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<ClientSingleTon>();

                if (_instance == null)
                {
                    return null;
                }

                return _instance;
            }
        }
        public ClientGameManager GameManager { get; private set; }

        public async Task<bool> CreateClientAsync()
        {
            GameManager = new ClientGameManager();
            return await GameManager.InitAsync();
        }

        private void OnDestroy()
        {
            GameManager?.Dispose();
        }
    }
}