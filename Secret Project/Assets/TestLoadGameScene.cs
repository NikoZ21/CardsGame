using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLoadGameScene : NetworkBehaviour
{
    [SerializeField] private float timer = 10f;

    private void Update()
    {
        if (!IsServer) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}