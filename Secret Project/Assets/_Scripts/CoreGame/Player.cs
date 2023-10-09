using _Scripts.Networking.Host;
using _Scripts.Networking.Shared;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject hud;
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData =
                HostSingleTon.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

            PlayerName.Value = userData.UserName;
        }

        if (IsClient)
        {
            hud.SetActive(IsLocalPlayer);
        }
    }
}