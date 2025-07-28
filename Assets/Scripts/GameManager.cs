using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject waitingUI;
    [SerializeField] private GameObject youWinUI;

    private bool gameStarted = false;
    public string menuSceneName = "Launcher";
    private bool hasWon = false;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} joined");
        CheckStartCondition();
    }

    void CheckStartCondition()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2 && !gameStarted)
        {
            Debug.Log("Enough players! Starting game...");
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartGame()
    {
        gameStarted = true;

        if (waitingUI != null)
            waitingUI.SetActive(false);

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<Shooter>().enabled = true;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room.");
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            photonView.RPC("ShowWinScreen", RpcTarget.All);
        }
    }

    [PunRPC]
    void ShowWinScreen()
    {
        if (hasWon) return;
        hasWon = true;

        if (youWinUI != null)
            youWinUI.SetActive(true);

        Invoke(nameof(ReturnToMenu), 5f);
    }

    void ReturnToMenu()
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
