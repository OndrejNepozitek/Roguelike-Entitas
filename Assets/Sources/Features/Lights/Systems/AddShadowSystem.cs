namespace Assets.Sources.Features.Lights.Systems
{
	using System.Collections.Generic;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using View;

	[ExecutePhase(ExecutePhase.ReactToComponents)] // TODO: wrong.. it reacts to components but must be after view system.. what to do?
	[ExecutesAfter(typeof(AddViewSystem))]
	public sealed class AddShadowSystem : ReactiveSystem<GameEntity>
	{
		public AddShadowSystem(Contexts contexts) : base(contexts.game)
		{

		}

		protected override void Execute(List<GameEntity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.hasShadow)
				{
					entity.ReplaceShadow(45);
				} else
				{
					entity.AddShadow(45);
				}
			}
		}

		protected override bool Filter(GameEntity entity)
		{
			return true;
		}

		protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
		{
			return context.CreateCollector(GameMatcher.View.Added());
		}
	}
}