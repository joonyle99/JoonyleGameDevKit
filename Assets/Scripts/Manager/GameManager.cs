using System;
using UnityEngine;

/// <summary>
/// This is obviously an example and I have no idea what kind of game you're making.
/// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
/// </summary>
[Serializable]
public enum GameState
{
    Starting = 0,
    SpawningHeroes = 1,
    SpawningEnemies = 2,
    HeroTurn = 3,
    EnemyTurn = 4,
    Win = 5,
    Lose = 6,
}

/// <summary>
/// Nice, easy to understand 'enum-based' game manager.
/// For larger and more complex games, look into state machines.
/// But this will serve just fine for most games.
/// </summary>
public class GameManager : JoonyleGameDevKit.Singleton<GameManager>
{
    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    // Kick the game off with the first state
    private void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        CurrentState = newState;

        switch (newState)
        {
            case GameState.Starting:
                Debug.Log("Game is starting");
                break;
            case GameState.SpawningHeroes:
                Debug.Log("Spawning heroes");
                break;
            case GameState.SpawningEnemies:
                Debug.Log("Spawning enemies");
                break;
            case GameState.HeroTurn:
                Debug.Log("Hero's turn");
                break;
            case GameState.EnemyTurn:
                Debug.Log("Enemy's turn");
                break;
            case GameState.Win:
                Debug.Log("You win!");
                break;
            case GameState.Lose:
                Debug.Log("You lose!");
                break;
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private void HandleStarting()
    {
        // Do some start setup, could be environment, cinematics etc

        // Eventually call ChangeState again with your next state

        ChangeState(GameState.SpawningHeroes);
    }
    private void HandleSpawningHeroes()
    {
        // ExampleUnitManager.Instance.SpawnHeroes();

        ChangeState(GameState.SpawningEnemies);
    }
    private void HandleSpawningEnemies()
    {
        // Spawn enemies

        ChangeState(GameState.HeroTurn);
    }
    private void HandleHeroTurn()
    {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc

        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.
    }
}
