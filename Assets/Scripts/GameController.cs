using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Systems _systems;
    public GameObject cameraObject;

    void Start()
    {
        Map.Instance = new Map(20,20);

        // get a reference to the contexts
        var contexts = Contexts.sharedInstance;

        contexts.game.SetEventQueue(new EventQueue<GameEntity>());
        contexts.game.SetCamera(cameraObject.GetComponent<Camera>());
        contexts.game.isPlayer = true;
        contexts.game.playerEntity.AddPosition(new IntVector2(7, 7));

        // create the systems by creating individual features
        _systems = new Feature("Systems")
            .Add(new ProcGenSystems(contexts))
            .Add(new StatsFeature(contexts))
            .Add(new CoroutinesSystems(contexts))
            .Add(new SmoothMovementSystem(contexts))
            .Add(new ActionSystem(contexts))
            .Add(new TurnFeature(contexts))
            .Add(new ViewSystems(contexts))
            .Add(new AIFeature(contexts))
            .Add(new RemoveInitSystem(contexts))
            .Add(new EnergySystem(contexts))
            .Add(new EntitiesDieOnMovementSystem(contexts))
            .Add(new PlayerCentricCameraSystem(contexts));

        

        /*var entity = contexts.game.CreateEntity();
        entity.isGameBoard = true;
        entity.AddEventQueue(new EventQueue<GameEntity>());*/

        // call Initialize() on all of the IInitializeSystems
        _systems.Initialize();
    }

    void Update()
    {
        // call Execute() on all the IExecuteSystems and 
        // ReactiveSystems that were triggered last frame
        _systems.Execute();
        // call cleanup() on all the ICleanupSystems
        _systems.Cleanup();
    }
}