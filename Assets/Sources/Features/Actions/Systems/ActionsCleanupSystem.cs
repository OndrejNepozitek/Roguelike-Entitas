﻿namespace Assets.Sources.Features.Actions.Systems
{
	using Entitas;

	/// <summary>
	/// Cleans actions at the end of each update cycle.
	/// </summary>
	public class ActionsCleanupSystem : ICleanupSystem
	{
		private readonly IGroup<ActionsEntity> group;

		public ActionsCleanupSystem(Contexts contexts)
		{
			var context = contexts.actions;
			group = context.GetGroup(ActionsMatcher.Action);
		}

		public void Cleanup()
		{
			foreach (var entity in group.GetEntities())
				entity.Destroy();
		}
	}
}