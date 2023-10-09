using System.Linq;
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
        private int playerCount;

        public void AddPlayerWrapper()
        {
            if (!IsClient) return;

            var player = FindObjectsByType<Player>(FindObjectsSortMode.None).FirstOrDefault(p => p == IsLocalPlayer);
            //var name = player.PlayerName.Value.ToString();
            AddPlayerToTeamServerRpc("Goggia");
        }

        [ServerRpc]
        private void AddPlayerToTeamServerRpc(string name)
        {
            if (playerCount == 2) return;
            playerCount++;
            UpdateUIClientRpc(name, playerCount);
        }

        [ClientRpc]
        private void UpdateUIClientRpc(string name, int count)
        {
            chooseBtn.interactable = count < 2;
            var nameTag = nameTags.FirstOrDefault(nt => nt.text == "Empty...");
            nameTag.text = name;
        }
    }

    public enum TeamColor
    {
        Blue,
        Red
    }
}