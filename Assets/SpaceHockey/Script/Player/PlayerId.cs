using System;
using UniRx;
using UnityEngine;

namespace SpaceHockey.Players
{
    public class PlayerId : Photon.MonoBehaviour
    {
        public static PlayerId Instance;
        public int Player_Id { get; private set; }
        public IObservable<int> OnInitializeAsync { get { return _onInitializeAsyncSubject; } }
        private readonly AsyncSubject<int> _onInitializeAsyncSubject = new AsyncSubject<int>();


        private void Awake()
        {
            Instance = this;
        }

        public void InitializePlayer()
        {
            Player_Id = PhotonNetwork.player.ID;
            _onInitializeAsyncSubject.OnNext(Player_Id);
            _onInitializeAsyncSubject.OnCompleted();

            switch (Player_Id)
            {
                case 1:
                    PhotonNetwork.Instantiate("1P_Racket", new Vector3(0, 1, 20), this.transform.rotation, 0);
                    break;
                case 2:
                    PhotonNetwork.Instantiate("2P_Racket", new Vector3(0, 1, -20), this.transform.rotation, 0);
                    break;
                default:
                    break;
            }
        }
    }
}