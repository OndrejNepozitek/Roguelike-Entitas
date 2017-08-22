namespace Assets.Sources.Features.Actions
{
	using Entitas;
	using UnityEngine;
	using System.Collections.Generic;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	/// <summary>
	/// Destroy actions that are not valid.
	/// </summary>
	/// <remarks>
	/// The motivation is that this system can run on both the client 
	/// and the server so the client does not spam with not needed actions.
	/// It also makes it systematic to validate actions.
	/// </remarks>
	[ExecutePhase(ExecutePhase.ValidateActions)]
	public class ValidateActionsSystem : ReactiveSystem<ActionsEntity>
	{
		private readonly GameContext gameContext;

		public ValidateActionsSystem(Contexts contexts) : base(contexts.actions)
		{
			gameContext = contexts.game;
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action.Added());
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (!entity.action.Action.Validate(gameContext))
				{
					Debug.Log("Destroying entity because of validation - " + entity.action.Action.GetType().Name);
					entity.Destroy();
				}
			}
		}
	}
}