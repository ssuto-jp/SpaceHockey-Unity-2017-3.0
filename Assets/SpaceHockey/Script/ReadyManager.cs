using System.Collections;
using System.Collections.Generic;
using SpaceHockey.Players;
using PhotonRx;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceHockey.GameManagers
{
    public class ReadyManager : Photon.MonoBehaviour
    {
        [SerializeField] private Text connectText;
        [SerializeField] private Text idText;

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings("1.0");

            this.OnJoinedLobbyAsObservable()
                .Subscribe(_ => PhotonNetwork.JoinRandomRoom());

            this.OnPhotonRandomJoinFailedAsObservable()
                .Subscribe(_ =>
                {
                    RoomOptions roomOptions = new RoomOptions()
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
                    PlayerId.Instance.OnInitialize();
                    idText.text = PlayerId.Instance.Player_Id.ToString() + "P";
                    Debug.Log("ルームに入室しました。");
                });

            this.OnPhotonPlayerConnectedAsObservable()
                .Subscribe(_ => Debug.Log("誰かがルームに入室しました。"));

            this.UpdateAsObservable()
                .Subscribe(_ => connectText.text = PhotonNetwork.connectionStateDetailed.ToString());
        }
    }
}

