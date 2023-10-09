using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _Scripts.Networking.Server;
using _Scripts.Networking.Shared;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Networking.Host
{
    public class HostGameManager : IDisposable
    {
        public NetworkServer NetworkServer { get; private set; }
        private Allocation allocation;
        private string joinCode;
        private string lobbyId;

        private const int MAXCONNECTIONS = 4;
        private const string LOBBYSCENENAME = "Lobby";

        public async Task StartHostAsync()
        {
            try
            {
                allocation = await Relay.Instance.CreateAllocationAsync(MAXCONNECTIONS);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            try
            {
                joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log(joinCode);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return;
            }

            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            RelayServerData relayServerData = new RelayServerData(allocation, "udp");
            transport.SetRelayServerData(relayServerData);

            try
            {
                CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
                lobbyOptions.IsPrivate = false;
                lobbyOptions.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member,
                            value: joinCode)
                    }
                };
                Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(
                    $"{PlayerPrefs.GetString(NameSelector.PLAYERNAMEKEY)}Lobby", MAXCONNECTIONS, lobbyOptions);

                lobbyId = lobby.Id;

                HostSingleTon.Instance.StartCoroutine(HearbeatLobby(15));
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                return;
            }

            NetworkServer = new NetworkServer(NetworkManager.Singleton);

            UserData userData = new UserData
            {
                UserName = PlayerPrefs.GetString(NameSelector.PLAYERNAMEKEY, "Missing Name"),
                UserAuthId = AuthenticationService.Instance.PlayerId
            };
            string payload = JsonUtility.ToJson(userData);
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

            NetworkManager.Singleton.StartHost();

            NetworkManager.Singleton.SceneManager.LoadScene(LOBBYSCENENAME, LoadSceneMode.Single);
        }

        private IEnumerator HearbeatLobby(float waitTimeSeconds)
        {
            WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds);
            while (true)
            {
                Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
            }
        }

        public async void Dispose()
        {
            HostSingleTon.Instance.StopCoroutine(nameof(HearbeatLobby));

            if (!string.IsNullOrEmpty(lobbyId))
            {
                try
                {
                    await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }

                lobbyId = string.Empty;
            }

            NetworkServer?.Dispose();
        }
    }
}