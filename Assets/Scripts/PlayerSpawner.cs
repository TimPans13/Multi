using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public float spawnRadius = 10f;
    public float spawnInterval = 5f;
    public GameObject[] playerPrefabs; 

    private PhotonView photonView;
    private GameController gameController;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            CreatePlayer();
        }
    }

    private void CreatePlayer()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber; 

        if (playerNumber >= 1 && playerNumber <= playerPrefabs.Length)
        {
            int prefabIndex = playerNumber - 1;
            GameObject playerPrefab = playerPrefabs[prefabIndex];

            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0f;

            Hashtable playerCustomProperties = new Hashtable
            {
                { "PlayerPrefabIndex", prefabIndex }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProperties);

            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Неверное количество игроков в комнате. Ожидалось от 1 до " + playerPrefabs.Length + " игроков.");
        }
    }
}
