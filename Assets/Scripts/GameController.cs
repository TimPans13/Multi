using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{
    private  GameObject lastPlayer;
    private bool isGameStart = false;
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private Text winnerPanelText;
    private static int deadPlayersCounter = 0;
    private int winnerCoinCount = 0;

    private void Start()
    {
        //Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isGameStart)
        {
            if ((PhotonNetwork.CurrentRoom.PlayerCount - deadPlayersCounter) == 1 && isGameStart) { photonView.RPC("EndGame", RpcTarget.AllBuffered); } 
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && !isGameStart)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("StartGame", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void StartGame()
    {
        isGameStart = true;
        coinManager.StartSpawnCoins();
    }

    [PunRPC]
    private void EndGame()
    {
        GetLastPlayer();
        photonView.RPC("ActivatePanel", RpcTarget.AllBuffered,winnerCoinCount);
        Time.timeScale = 0f;//time stop(idk use it or not)
    }


    [PunRPC]
    private void ActivatePanel(int coins)
    {

            winnerPanel.SetActive(true);
            if (lastPlayer != null)
            {
                PlayerController playerController = lastPlayer.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    if (lastPlayer != null)
                    {
                        winnerPanelText.text = (playerController.photonView.ToString() + "     coins =  " + coins.ToString());
                    }
                    else winnerPanelText.text = "NENAHOD";
                }
            }
    }

    [PunRPC]
    public void IncreaseDeadPlayersCounter()
    {
        deadPlayersCounter += 1;
    }

    [PunRPC]
    private void GetLastPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject lastAlivePlayer = null;
        int maxCoins = -1; 

        foreach (GameObject player in players)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController != null && playerController.IsAlive())
            {
                int playerCoins = playerController.GetPlayerCoinCounter();
                {
                    maxCoins = playerCoins;
                    lastAlivePlayer = player;
                }
            }
        }

        if (lastAlivePlayer != null)
        {
            lastPlayer = lastAlivePlayer;
            winnerCoinCount = maxCoins; 
        }
    }

}
