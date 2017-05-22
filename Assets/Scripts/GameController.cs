using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Systems _systems;
    public GameObject cameraObject;

    void Start()
    {
        Map.Instance = new Map(100,100);

        // get a reference to the contexts
        var contexts = Contexts.sharedInstance;

        contexts.game.SetEventQueue(new EventQueue<GameEntity>());
        contexts.game.SetCamera(cameraObject.GetComponent<Camera>());
        contexts.game.isPlayer = true;
        //contexts.game.playerEntity.AddPosition(new IntVector2(7, 7));
        //contexts.game.playerEntity.AddSmoothMovement(new IntVector2(7, 7), 0.5f);

        contexts.game.isGameBoard = true;
        contexts.game.gameBoardEntity.AddRectangularMap(100, 100);

        // create the systems by creating individual features
        _systems = new Feature("Systems")

            .Add(new ProcGenFeature(contexts))                  // Initial world generation
            .Add(new StatsFeature(contexts))                    // Marks all dead entities and removes then on cleanup.
            .Add(new InputFeature(contexts))
            .Add(new MapTrackerSystem(contexts))

            // Systems which generate actions
            // Should be placed before consumers
            .Add(new CoroutinesSystems(contexts))               // May create actions as a result of coroutine
            .Add(new AIFeature(contexts))                       // Should be placed before Movement actions as it changes position and creates Attack actions
            .Add(new SmoothMovementSystem(contexts))            // Must be placed before View systems as it changes position
            .Add(new ViewSystems(contexts))                     // Creates Move actions

            // Systems which react to actions
            .Add(new EnergySystem(contexts))                    // Reacts to actions and handle energy costs based on entities' Stats

            // Other systems
            //.Add(new EntitiesDieOnMovementSystem(contexts))     // System to test health system. Entities are damaged as they move
            .Add(new PlayerCentricCameraSystem(contexts))       // Makes sure that camera is centered on the player
            .Add(new TurnFeature(contexts))                     // Works with energy to queue entities
            .Add(new FogOfWarFeature(contexts))
            .Add(new LightsFeature(contexts))

            // Cleanup systems
            .Add(new RemoveInitSystem(contexts))                // Removes Init flag from all entities
            .Add(new CombatSystem(contexts))                    // Combat system should be placed right before action cleanup
            .Add(new LogSystem(contexts))
            .Add(new ActionSystem(contexts));                   // Actions cleanup


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