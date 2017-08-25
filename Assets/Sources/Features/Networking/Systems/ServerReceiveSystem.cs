namespace Assets.Sources.Features.Networking.Systems
{
	using System;
	using Actions;
	using Entitas;
	using Helpers.Networking;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;

	/// <summary>
	/// Fetch all actions from client at the beginning of each update cycle.
	/// Send all actions to client at the end of every update cycle.
	/// </summary>
	[ExecutePhase(ExecutePhase.Init)]
	[DependsOn(typeof(ActionsFeature))]
	public class ServerReceiveSystem : IExecuteSystem
	{
		private readonly ActionsContext actionsContext;

		public ServerReceiveSystem(Contexts contexts)
		{
			actionsContext = contexts.actions;
		}

		public void Execute()
		{
			if (!(NetworkController.Instance.NetworkEntity is Server)) return;

			var actions = NetworkController.Instance.NetworkEntity.Actions;

			if (actions == null) return;

			foreach (var action in actions)
			{
				if (action == null)
				{
					throw new ArgumentException("Action must not be null");
				}

				var entity = actionsContext.CreateEntity();
				entity.AddAction(action);
			}
		}
	}
}
