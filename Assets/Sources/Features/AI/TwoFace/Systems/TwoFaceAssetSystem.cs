namespace Assets.Sources.Features.AI.TwoFace.Systems
{
	using System.Collections.Generic;
	using Actions;
	using Entitas;
	using Helpers;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	[ExecutePhase(ExecutePhase.ReactToActions)]
	public class TwoFaceAssetSystem : ReactiveSystem<ActionsEntity>
	{
		private readonly GameContext gameContext;
		private readonly IGroup<GameEntity> twoFacers;

		public TwoFaceAssetSystem(Contexts contexts) : base(contexts.actions)
		{
			gameContext = contexts.game;
			twoFacers = contexts.game.GetGroup(GameMatcher.TwoFace);
		}

		protected override ICollector<ActionsEntity> GetTrigger(IContext<ActionsEntity> context)
		{
			return context.CreateCollector(ActionsMatcher.Action);
		}

		protected override bool Filter(ActionsEntity entity)
		{
			return entity.hasAction && entity.action.Action is TwoFaceChangeAction;
		}

		protected override void Execute(List<ActionsEntity> entities)
		{
			foreach (var entity in entities)
			{
				var action = (TwoFaceChangeAction) entity.action.Action;

				// TODO: it should be DRY
				if (gameContext.hasTwoFaceState)
				{
					gameContext.ReplaceTwoFaceState(gameContext.twoFaceState.TimeElapsed, action.IsAngry);
				}
				else
				{
					gameContext.SetTwoFaceState(gameContext.twoFaceState.TimeElapsed, action.IsAngry);
				}

				foreach (var twoFacer in twoFacers.GetEntities())
				{
					string asset;

					if (action.IsAngry)
					{
						asset = Prefabs.MonsterGreen;
					}
					else
					{
						asset = Prefabs.BodyBrown;
					}

					if (twoFacer.hasAsset)
					{
						twoFacer.ReplaceAsset(asset);
					}
					else
					{
						twoFacer.AddAsset(asset);
					}
				}
			}
		}
	}
}