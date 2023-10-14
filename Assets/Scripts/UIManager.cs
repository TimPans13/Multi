using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text healthText;
    public Text coinText;
    public Image healthBar;
    [SerializeField] private Button exitButton;
    //public Image healthText;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitToLobby);
    }

    public void ExitToLobby()
    {
        PhotonNetwork.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }
}