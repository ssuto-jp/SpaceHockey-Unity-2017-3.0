using SpaceHockey.Players;
using PhotonRx;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceHockey.GameManagers
{
    public class ReadyManager : MonoBehaviour
    {
        private PhotonView photonView;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private Text loadText;
        [SerializeField] private Text connectText;
        [SerializeField] private Text idText;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void ConnectNetwork()
        {
            PhotonNetwork.ConnectUsingSettings("1.0");

            this.OnJoinedLobbyAsObservable()
                .Subscribe(_ => PhotonNetwork.JoinRandomRoom());

            this.OnPhotonRandomJoinFailedAsObservable()
                .Subscribe(_ =>
                {
                    var roomOptions = new RoomOptions()
                    {
                        MaxPlayers = 2,
                        IsOpen = true,
                        IsVisible = true,
                    };
                    PhotonNetwork.CreateRoom(null, roomOptions, null);
                    Debug.Log("ルームを作成しました。");
                });

            this.OnJoinedRoomAsObservable()
                .Subscribe(_ =>
                {
                    PlayerId.Instance.InitializePlayer();
                    idText.text = PlayerId.Instance.Player_Id.ToString() + "P";
                    Debug.Log("ルームに入室しました。");
                });

            this.OnPhotonPlayerDisconnectedAsObservable()
                .Subscribe(_ =>
                {
                    PhotonNetwork.Disconnect();
                    SceneManager.LoadScene("Title");
                    Debug.Log("通信が切断されました。");
                });

            this.UpdateAsObservable()
                 .Subscribe(_ => connectText.text = PhotonNetwork.connectionStateDetailed.ToString());
        }

        public void OnFinishedLoading()
        {
            photonView.RPC("HiddenReadyPanel", PhotonTargets.All);
        }

        [PunRPC]
        private void HiddenReadyPanel()
        {
            readyPanel.SetActive(false);
        }
    }
}

