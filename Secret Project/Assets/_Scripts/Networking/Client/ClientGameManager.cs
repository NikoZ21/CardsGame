using System;
using System.Text;
using System.Threading.Tasks;
using _Scripts.Networking.Shared;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Networking.Client
{
    public class ClientGameManager : IDisposable
    {
        private NetworkClient networkClient;
        private JoinAllocation joinAllocation;
        private const string MENUSCENENAME = "MainMenu";

        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();

            networkClient = new NetworkClient(NetworkManager.Singleton);

            AuthState authState = await AuthenticationWrapper.DoAuthAsync();

            if (authState == AuthState.Authenticated)
            {
                return true;
            }

            return false;
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(MENUSCENENAME);
        }

        public async Task StartClientAsync(string joinCode)
        {
            try
            {
                joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "udp");
            transport.SetRelayServerData(relayServerData);

            UserData userData = new UserData
            {
                UserName = PlayerPrefs.GetString(NameSelector.PLAYERNAMEKEY, "Missing Name"),
                UserAuthId = AuthenticationService.Instance.PlayerId,
            };
            string payload = JsonUtility.ToJson(userData);
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

            NetworkManager.Singleton.StartClient();
        }

        public void Dispose()
        {
            networkClient?.Dispose();
        }
    }
}