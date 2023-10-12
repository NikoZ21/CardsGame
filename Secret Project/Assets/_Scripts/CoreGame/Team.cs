using System.Collections.Generic;
using System.Linq;
using _Scripts.Networking.Host;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

namespace _Scripts.CoreGame
{
    public class Team : NetworkBehaviour
    {
        [SerializeField] private Button chooseBtn;
        [SerializeField] private TeamColor teamColor;
        [SerializeField] private TextMeshProUGUI[] nameTags;
        private List<Player> players = new List<Player>();
        private static int playersReady;

        public override void OnNetworkSpawn()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void AddPlayerWrapper()
        {
            var localPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);

            foreach (Player p in localPlayers)
            {
                Debug.Log(localPlayers.Length);

                if (p.IsLocalPlayer)
                {
                    AddPlayerToTeamServerRpc(p.OwnerClientId);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddPlayerToTeamServerRpc(ulong clientId)
        {
            var player = HostSingleTon.Instance.GameManager.NetworkServer.ClienIdToPlayer[clientId];
            players.Add(player);
            playersReady++;
            UpdateUIClientRpc(player.PlayerName.Value.ToString(), players.Count);

            if (playersReady == 2)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Game",
                    LoadSceneMode.Single);
            }
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