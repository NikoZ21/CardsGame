using System.Collections.Generic;
using System.Linq;
using _Scripts.Networking.Host;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace _Scripts.CoreGame
{
    public class Team : NetworkBehaviour
    {
        [SerializeField] private Button chooseBtn;
        [SerializeField] private TeamColor teamColor;
        [SerializeField] private TextMeshProUGUI[] nameTags;
        private List<Player> players = new List<Player>();

        public void AddPlayerWrapper()
        {
            var localPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);

            foreach (Player p in localPlayers)
            {
                Debug.Log(localPlayers.Length);

                if (p.IsLocalPlayer)
                {
                    Debug.Log(p.PlayerName.Value);
                    AddPlayerToTeamServerRpc(p.OwnerClientId);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddPlayerToTeamServerRpc(ulong clientId)
        {
            var player = HostSingleTon.Instance.GameManager.NetworkServer.ClienIdToPlayer[clientId];
            players.Add(player);
            UpdateUIClientRpc(player.PlayerName.Value.ToString(), players.Count);
        }

        [ClientRpc]
        private void UpdateUIClientRpc(string n, int c)
        {
            Debug.Log("updating UI");

            chooseBtn.interactable = c < 2;
            var nameTag = nameTags.FirstOrDefault(nt => nt.text == "Empty...");
            nameTag.text = n;
        }
    }

    public enum TeamColor
    {
        Blue,
        Red
    }
}