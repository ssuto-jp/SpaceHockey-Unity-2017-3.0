﻿using PhotonRx;
using UniRx;
using UnityEngine;

namespace SpaceHockey.GameManagers
{
    [RequireComponent(typeof(ReadyManager))]
    [RequireComponent(typeof(BattleManager))]
    [RequireComponent(typeof(ResultManager))]
    public class MainGameManager : MonoBehaviour
    {
        private ReadyManager readyManager;
        private BattleManager battleManager;
        private ResultManager resultManager;

        private GameStateReactiveProperty _currentGameState = new GameStateReactiveProperty(GameState.Ready);
        public IReadOnlyReactiveProperty<GameState> CurrentGameState
        {
            get { return _currentGameState; }
        }

        private void Start()
        {
            readyManager = GetComponent<ReadyManager>();
            battleManager = GetComponent<BattleManager>();
            resultManager = GetComponent<ResultManager>();

            CurrentGameState
                .Subscribe(state => OnStateChanged(state));
        }

        private void OnStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Ready:
                    OnReady();
                    break;
                case GameState.Battle:
                    OnBattle();
                    break;
                case GameState.Result:
                    OnResult();
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
                    readyManager.OnFinishedLoading();
                    _currentGameState.Value = GameState.Battle;
                });
        }

        private void OnBattle()
        {
            battleManager.StartBattle();
            battleManager.IsWinner
                .SkipWhile(b => b != true)
                .DistinctUntilChanged()
                .Subscribe(_ => _currentGameState.Value = GameState.Result);
        }

        private void OnResult()
        {
            resultManager.StartResult();
        }
    }
}

