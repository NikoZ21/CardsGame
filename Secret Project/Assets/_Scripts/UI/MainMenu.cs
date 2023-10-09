using _Scripts.Networking.Client;
using _Scripts.Networking.Host;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinCode;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject joinRoomPanel;
        [SerializeField] private GameObject nameSelectPanel;

        public async void JoinRoomAsync()
        {
            await ClientSingleTon.Instance.GameManager.StartClientAsync(joinCode.text);
        }

        public async void StartRoomAsync()
        {
            await HostSingleTon.Instance.GameManager.StartHostAsync();
        }
    }
}