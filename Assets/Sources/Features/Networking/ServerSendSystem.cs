namespace Assets.Sources.Features.Networking
{
	using System;
	using System.Linq;
	using Actions;
	using Helpers.Networking;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using UnityEngine;

	/// <summary>
	/// Fetch all actions from client at the beginning of each update cycle.
	/// Send all actions to client at the end of every update cycle.
	/// </summary>
	[ExecutePhase(ExecutePhase.Cleanup)]
	[DependsOn(typeof(ActionsFeature))]
	public class ServerSendSystem : IExecuteSystem
	{
		private readonly ActionsContext actionsContext;

		public ServerSendSystem(Contexts contexts)
		{
			actionsContext = contexts.actions;
		}

		public void Execute()
		{
			var server = NetworkController.Instance.NetworkEntity as Server; // TODO: ugly, slow
			if (server != null)
			{
				var actions = actionsContext.GetEntities().Where(e => e.hasAction).Select(e => e.action.Action).ToList();

				if (actions.Count != 0)
				{
					Debug.Log(string.Format("Sending {0} actions", actions.Count));
					server.SendActions(actions);
				}
			}
		}
	}
}