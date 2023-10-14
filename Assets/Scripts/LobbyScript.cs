using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    public InputField createRoomInput;
    public InputField joinRoomInput;
    public Button createRoomButton;
    public Button joinRoomButton;

    private void Start()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    public void CreateRoom()
    {
        string roomName = createRoomInput.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
        }
    }

    public void JoinRoom()
    {
        string roomName = joinRoomInput.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully.");
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message);
    }
}