using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Networking.Client
{
    public class ClientGameManager
    {
        private JoinAllocation _joinAllocation;
        private const string MenuSceneName = "MainMenu";

        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();

            AuthState authState = await AuthenticationWrapper.DoAuthAsync();

            if (authState == AuthState.Authenticated)
            {
                return true;
            }

            return false;
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(MenuSceneName);
        }

        public async Task StartClientAsync(string joinCode)
        {
            try
            {
                _joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            RelayServerData relayServerData = new RelayServerData(_joinAllocation, "udp");
            transport.SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
    }
}