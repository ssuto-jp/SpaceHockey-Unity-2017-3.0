using System;
using UniRx;
using UnityEngine;

namespace SpaceHockey.GameManagers
{
    public enum GameState
    {
        Ready,
        Battle,
        Result,
    }

    [Serializable]
    public class GameStateReactiveProperty : ReactiveProperty<GameState>
    {
        public GameStateReactiveProperty()
        {
        }

        public GameStateReactiveProperty(GameState initialValue)
            : base(initialValue)
        {
        }
    }
}

