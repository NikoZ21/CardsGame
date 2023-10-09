using System.Threading.Tasks;
using _Scripts.Networking.Client;
using _Scripts.Networking.Host;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Networking.Shared
{
    public class ApplicationController : MonoBehaviour
    {
        [SerializeField] private ClientSingleTon clientSingleTon;
        [SerializeField] private HostSingleTon hostSingleTon;

        async void Start()
        {
            DontDestroyOnLoad(gameObject);

            HostSingleTon hostPrefab = Instantiate(hostSingleTon, transform);
            hostPrefab.CreateHost();

            ClientSingleTon clientPrefab = Instantiate(clientSingleTon, transform);
            bool isAuthenticated = await clientPrefab.CreateClientAsync();

            if (isAuthenticated)
            {
                ClientSingleTon.Instance.GameManager.GoToMenu();
            }
        }
    }
}