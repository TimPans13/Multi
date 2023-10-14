using UnityEngine;
using Photon.Pun;

public class CoinManager : MonoBehaviourPunCallbacks
{
    public GameObject Coin;
    public int maxCoins = 10;
    public float spawnRadius = 7f;

    private void Start()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    //SpawnCoins();
        //}
    }

    private void SpawnCoins()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < maxCoins; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
                //randomPosition.y = 0f;
                GameObject coin = PhotonNetwork.Instantiate("Coin", randomPosition, Quaternion.identity);
            }
        }
    }
    public void StartSpawnCoins()
    {
        SpawnCoins();
    }

    public void DestroyCoin(GameObject coin)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(coin);
        }
    }
}