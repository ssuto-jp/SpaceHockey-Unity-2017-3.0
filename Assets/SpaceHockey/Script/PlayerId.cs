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
        }

    }
}