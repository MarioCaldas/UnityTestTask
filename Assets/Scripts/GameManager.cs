using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI gameOverText;

    PhotonView PV;

    [SerializeField] private int maxDeathsGameOver;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public int GetMaxDeathsGameOver()
    {
        return maxDeathsGameOver;
    }

    public void GameOver(string playerName)
    {
        PV.RPC(nameof(RPC_GameOver), RpcTarget.All, playerName);
    }

    [PunRPC]
    public void RPC_GameOver(string playerName)
    {
        gameOverCanvas.SetActive(true);
        gameOverText.text = playerName + " Won";
        PhotonNetwork.LeaveRoom();
    }
}
