using System.Collections;
using System.Collections.Generic;
using PhotonRx;
using UniRx;
using UnityEngine;

namespace SpaceHockey.GameManagers
{
    public class MainGameManager : Photon.MonoBehaviour, IGameStateProvider
    {
        private ReadyManager readyManager;

        private GameStateReactiveProperty _currentGameState = new GameStateReactiveProperty(GameState.Ready);
        public IReadOnlyReactiveProperty<GameState> CurrentGameState
        {
            get
            {
                return _currentGameState;
            }
        }

        private void Start()
        {
            readyManager = GetComponent<ReadyManager>();

            CurrentGameState
                .Subscribe(state => OnStateChanged(state));
        }

        void OnStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Ready:
                    Debug.Log("Ready");
                    OnReady();
                    break;
                case GameState.Battle:
                    Debug.Log("Battle");
                    OnBattle();
                    break;
                case GameState.Result:
                    Debug.Log("Result");
                    break;
                default:
                    break;
            }
        }

        private void OnReady()
        {
            readyManager.ConnectNetwork();
            this.OnPhotonPlayerConnectedAsObservable()
                .Where(_ => PhotonNetwork.playerList.Length == 2)
                .Subscribe(_ =>
                {
                    Debug.Log("誰かがルームに入室しました。");
                    _currentGameState.Value = GameState.Battle;
                });
        }

        private void OnBattle()
        {
            //
        }

        private void OnResult()
        {
            //
        }
    }
}

