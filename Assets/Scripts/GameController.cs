using System;
using System.Collections.Generic;
using Assets.Sources.Helpers;
using Assets.Sources.Helpers.Networking;
using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameState GameState { get; private set; }

    Systems systems;
    public GameObject CameraObject;
	public GameObject StartGameOverlay;

	public GameController()
	{
		GameState = GameState.NotStarted;
	}

	public void StartGame()
	{
		StartGameOverlay.SetActive(false);
		GameState = GameState.Running;
	}

	public void InitGame()
	{
		GameState = GameState.WaitingForPlayers;
		Application.targetFrameRate = 60;

		Map.Instance = new Map(100, 100);

		// get a reference to the contexts
		var contexts = Contexts.sharedInstance;

		contexts.game.SetEventQueue(new EventQueue<GameEntity>());
		contexts.game.SetCamera(CameraObject.GetComponent<Camera>());
		
		//contexts.game.playerEntity.AddPosition(new IntVector2(7, 7));
		//contexts.game.playerEntity.AddSmoothMovement(new IntVector2(7, 7), 0.5f);

		contexts.game.isGameBoard = true;
		contexts.game.gameBoardEntity.AddRectangularMap(100, 100);

		// create the systems by creating individual features
		systems = new Feature("Systems");



		// New order
		// Initialization
		systems
			.Add(new MapTrackerSystem(contexts)) // This system has only a constructor
			.Add(new RegisterItemsSystem()) // Creates item database
			.Add(new RegisterMonstersSystem())
			.Add(new ProcGenFeature(contexts)) // Initial world generation TODO check
			.Add(new StatsFeature(contexts)) // Marks all dead entities and removes then on cleanup. TODO check
			.Add(new NetworkTrackingSystem(contexts)) // TODO possible violation of these rules
			.Add(new ServerSystem(contexts));


		// Input handling
		systems
			.Add(new InputFeature(contexts));

		// Process actions and dispatch - actions can be changed, do not change entities
		// This happens only on the server-side
		if (NetworkController.Instance.IsMultiplayer)
		{
			if (NetworkController.Instance.NetworkEntity is Server)
			{
				systems
					.Add(new AIFeature(contexts))
					.Add(new AddMonsterReferenceSystem(contexts));
			}
		}

		// Validate actions
		systems.Add(new ValidateActionsSystem(contexts));


		if (NetworkController.Instance.IsMultiplayer && !(NetworkController.Instance.NetworkEntity is Server))
		{
			systems
				.Add(new ClientSystem(contexts));
		}

		// React to actions - do not change actions, entites may be changed
		// TODO: maybe allow destroying actions? but before any changes were made
		systems
			.Add(new ProcessBasicMoveSystem(contexts))
			.Add(new SpawnItemSystem(contexts))
			.Add(new EquipItemSystem(contexts))
			.Add(new SpawnMonsterSystem(contexts))

			// React to components changes
			.Add(new ViewSystems(contexts)) // May need to be revised
			.Add(new FogOfWarFeature(contexts))
			.Add(new LightsFeature(contexts))

			// Cleanup actions
			.Add(new PlayerCentricCameraSystem(contexts))
			.Add(new ActionsCleanupSystem(contexts))




			// Systems which generate actions
			// Should be placed before consumers
			.Add(new CoroutinesSystems(contexts)) // May create actions as a result of coroutine
			// .Add(new AIFeature(contexts))                       // Should be placed before Movement actions as it changes position and creates Attack actions
			// .Add(new ViewSystems(contexts))                     // Creates Move actions

			// Systems which react to actions
			// .Add(new EnergySystem(contexts))                    // Reacts to actions and handle energy costs based on entities' Stats

			// Other systems
			//.Add(new EntitiesDieOnMovementSystem(contexts))     // System to test health system. Entities are damaged as they move
			// .Add(new PlayerCentricCameraSystem(contexts))       // Makes sure that camera is centered on the player
			// .Add(new TurnFeature(contexts))                     // Works with energy to queue entities

			// .Add(new LightsFeature(contexts))

			// Cleanup systems
			.Add(new RemoveInitSystem(contexts)) // Removes Init flag from all entities
			.Add(new CombatSystem(contexts)) // Combat system should be placed right before ActionOld cleanup
			.Add(new LogSystem(contexts))
			.Add(new ActionSystem(contexts)) // Actions cleanup

			.Add(new RemoveViewSystem(contexts));


		// call Initialize() on all of the IInitializeSystems
		systems.Initialize();

		NetworkController.Instance.OnGameStarted += StartGame;
		NetworkController.Instance.SendWaitingForPlayers();
	}

	void Start()
    {
		if (GameState == GameState.NotStarted)
	    {
			InitGame();
	    }
    }

	private void OnDestroy()
	{
		NetworkController.Instance.OnGameStarted -= StartGame;
	}

    void Update()
    {
	    if (GameState == GameState.Running || (GameState == GameState.WaitingForPlayers && !NetworkController.Instance.IsServer))
	    {
			// call Execute() on all the IExecuteSystems and 
		    // ReactiveSystems that were triggered last frame
		    systems.Execute();
		    // call cleanup() on all the ICleanupSystems
		    systems.Cleanup();
		}
    }

	public void PauseGame()
	{
		GameState = GameState.Paused;
	}

	public void UnpauseGame()
	{
		GameState = GameState.Running;
	}

	public void StopGame()
	{
		GameState = GameState.NotStarted;

		if (systems != null)
		{
			systems.TearDown();
			systems.ClearReactiveSystems();
			systems.DeactivateReactiveSystems();
		}
		Contexts.sharedInstance.Reset();
	}
}