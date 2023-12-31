using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
	public static Launcher Instance;

	[SerializeField] TMP_InputField roomNameInput;
	[SerializeField] TMP_Text errorText;
	[SerializeField] TMP_Text roomNameText;
	[SerializeField] Transform roomListContent;
	[SerializeField] GameObject roomListItemPrefab;
	[SerializeField] Transform playerListContent;
	[SerializeField] GameObject PlayerListItemPrefab;
	[SerializeField] GameObject startGameButton;


	void Awake()
	{
		Instance = this;
	}
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
	}
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		MenuController.Instance.OpenMenu("title");
		PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
	}
	public void CreateRoom()
	{
		if (string.IsNullOrEmpty(roomNameInput.text))
		{
			return;
		}

		PhotonNetwork.CreateRoom(roomNameInput.text);
		MenuController.Instance.OpenMenu("loading");
	}
	public void JoinRoom(RoomInfo info)
	{		
		PhotonNetwork.JoinRoom(info.Name);
		MenuController.Instance.OpenMenu("loading");
	}

	public override void OnJoinedRoom()
	{
		MenuController.Instance.OpenMenu("room");
		roomNameText.text = PhotonNetwork.CurrentRoom.Name;

		Player[] players = PhotonNetwork.PlayerList;

		foreach (Transform child in playerListContent)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < players.Count(); i++)
		{
			Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
		}

		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		startGameButton.SetActive(PhotonNetwork.IsMasterClient);
	}

	public void StartGame()
	{
		if (PhotonNetwork.PlayerList.Length > 1)
			PhotonNetwork.LoadLevel(1);
	}

	public override void OnLeftRoom()
	{
		MenuController.Instance.OpenMenu("title");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		errorText.text = "Room Creation Failed: " + message;
		MenuController.Instance.OpenMenu("error");
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		MenuController.Instance.OpenMenu("loading");
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach (Transform trans in roomListContent)
		{
			Destroy(trans.gameObject);
		}

		for (int i = 0; i < roomList.Count; i++)
		{
			if (roomList[i].RemovedFromList)
				continue;
			Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
	}
}
