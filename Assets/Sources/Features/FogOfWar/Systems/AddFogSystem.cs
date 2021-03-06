﻿namespace Assets.Sources.Features.FogOfWar.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using View;

	[ExecutePhase(ExecutePhase.ReactToComponents)]
	[ExecutesAfter(typeof(AddViewSystem))]
	public sealed class AddFogSystem : ReactiveSystem<GameEntity>
	{
		public AddFogSystem(Contexts contexts) : base(contexts.game)
		{

		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (!entity.isInFog)
				{
					entity.isInFog = true;
					entity.view.gameObject.SetActive(false);
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return entity.hasView; // Maybe apply only to specified entities?
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.View.Added());
		}
	}
}