using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceHockey.GameManagers
{ 
    public interface IGameStateProvider
    {       
        IReadOnlyReactiveProperty<GameState> CurrentGameState { get; }
    }
}

