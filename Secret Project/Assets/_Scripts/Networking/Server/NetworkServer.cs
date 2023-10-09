using _Scripts.Networking.Shared;
using Unity.Netcode;
using UnityEngine;

namespace _Scripts.Networking.Server
{
    public class NetworkServer
    {
        private NetworkManager networkManager;

        public NetworkServer(NetworkManager networkManager)
        {
            this.networkManager = networkManager;

            networkManager.ConnectionApprovalCallback += ApprovalCheck;
        }

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
            UserData userData = JsonUtility.FromJson<UserData>(payload);

            Debug.Log(userData.UserName);

            response.Approved = true;
            response.CreatePlayerObject = true;
        }
    }
}