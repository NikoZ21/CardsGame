using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

namespace _Scripts.UI
{
    public class DisplayPlayerName : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private TMP_Text playerNameText;

        private void OnEnable()
        {
            HandlePlayerNameChanged(string.Empty, player.PlayerName.Value);

            player.PlayerName.OnValueChanged += HandlePlayerNameChanged;
        }

        private void HandlePlayerNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
        {
            playerNameText.text = newName.ToString();
        }

        private void OnDestroy()
        {
            player.PlayerName.OnValueChanged -= HandlePlayerNameChanged;
        }
    }
}