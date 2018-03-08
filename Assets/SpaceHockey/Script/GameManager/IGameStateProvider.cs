using UniRx;

namespace SpaceHockey.GameManagers
{
    public interface IGameStateProvider
    {
        IReadOnlyReactiveProperty<GameState> CurrentGameState { get; }
    }
}

