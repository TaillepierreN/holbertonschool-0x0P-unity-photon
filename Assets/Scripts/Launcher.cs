using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
	[SerializeField] private TMP_InputField nameInput;

	[SerializeField] private GameObject connectUI;
	[SerializeField] private GameObject WaitingUI;
	public Transform[] spawnPositions;
	public string gameVersion = "0.1";

	void Start()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.GameVersion = gameVersion;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to Photon!");
		connectUI.SetActive(true);
	}

	public void OnJoinButtonClicked()
	{
		PhotonNetwork.NickName = nameInput.text;
		PhotonNetwork.JoinRandomOrCreateRoom();

	}

	public override void OnJoinedRoom()
	{
		int playerIndex = PhotonNetwork.CurrentRoom.PlayerCount;
		string prefabName = playerIndex == 1 ? "Player" : $"Player{playerIndex}";
		int index = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % spawnPositions.Length;
		Vector3 spawnPos = spawnPositions[index].position;

		Debug.Log($"Instantiating prefab: {prefabName}");
		PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);
		connectUI.SetActive(false);
		WaitingUI.SetActive(true);
	}
}
