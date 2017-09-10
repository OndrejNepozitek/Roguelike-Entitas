namespace Assets.Sources.Features.AI.TwoFace.Systems
{
	using Actions;
	using Config;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	[ExecutePhase(ExecutePhase.ProcessActions)]
	public class TwoFaceTimerSystem : IExecuteSystem
	{
		private readonly GameContext gameContext;
		private readonly ActionsContext actionsContext;
		private readonly Config config;

		public TwoFaceTimerSystem(Contexts contexts)
		{
			gameContext = contexts.game;
			actionsContext = contexts.actions;
			config = gameContext.GetConfig();
		}

		public void Execute()
		{
			if (!gameContext.hasTwoFaceState)
			{
				gameContext.SetTwoFaceState(0, false);
				return;
			}

			gameContext.twoFaceState.TimeElapsed += Time.deltaTime;

			if (gameContext.twoFaceState.TimeElapsed >= config.TwoFaceChangeTime)
			{
				gameContext.twoFaceState.TimeElapsed = 0;

				var action = actionsContext.CreateEntity();
				action.AddAction(new TwoFaceChangeAction() { IsAngry = !gameContext.twoFaceState.IsAngry});
			}
		}
	}
}
