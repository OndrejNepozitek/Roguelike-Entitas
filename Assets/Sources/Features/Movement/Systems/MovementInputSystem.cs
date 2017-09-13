namespace Assets.Sources.Features.Movement.Systems
{
	using Entitas;
	using Extensions;
	using Helpers;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.Input)]
	public class MovementInputSystem : IExecuteSystem
	{
		private readonly ActionsContext actionsContext;
		private readonly GameContext gameContext;

		private bool lastHorizontal;

		public MovementInputSystem(Contexts contexts)
		{
			actionsContext = contexts.actions;
			gameContext = contexts.game;
		}

		public void Execute()
		{
			if (Input.GetKey(KeyCode.LeftControl)) return;

			// Handle moving
			// TODO: player should not be able to move while the game is loading
			var player = gameContext.GetCurrentPlayer();

			if (player == null)
			{
				return;
			}

			var horizontal = (int)Input.GetAxisRaw("Horizontal");
			var vertical = (int)Input.GetAxisRaw("Vertical");

			if (horizontal != 0 && vertical != 0)
			{
				if (lastHorizontal)
				{
					horizontal = 0;
				}
				else
				{
					vertical = 0;
				}
			}

			if (horizontal != 0)
			{
				lastHorizontal = true;
			}

			if (vertical != 0)
			{
				lastHorizontal = false;
			}

			var direction = IntVector2.GetGridDirection(horizontal, vertical);
			//var direction = new IntVector2(horizontal, vertical);

			if (direction != IntVector2.Empty)
			{
				actionsContext.BasicMove(player, player.position.value + direction);
			}
		}
	}
}
