namespace Assets.Sources.Features.Networking.Systems
{
	using System;
	using System.Collections.Generic;
	using Actions;
	using Entitas;
	using Helpers.SystemDependencies.Attributes;
	using Helpers.SystemDependencies.Phases;
	using Movement;
	using UnityEngine;

	/// <summary>
	/// Sends own actions (and destroyes them afterwards) and receives actions from server.
	/// </summary>
	[ExecutePhase(ExecutePhase.Network)]
	[DependsOn(typeof(ActionsFeature))]
	public class ClientSystem : IExecuteSystem
	{
		private readonly ActionsContext actionsContext;
		private readonly IGroup<ActionsEntity> group;

		public ClientSystem(Contexts contexts)
		{
			actionsContext = contexts.actions;
			group = actionsContext.GetGroup(ActionsMatcher.Action);
		}

		public void Execute()
		{
			var client = NetworkController.Instance.NetworkEntity;
			var actionsReceived = client.Actions;
			var actionsToBeSent = new List<IAction>();

			foreach (var entity in group.GetEntities())
			{
				if (entity.hasAction)
				{
					actionsToBeSent.Add(entity.action.Action);

					// Basic move prediction on client
					if (!(entity.action.Action is BasicMoveAction))
					{
						entity.Destroy();
					}
				}

				if (actionsToBeSent.Count > 0)
				{
					Debug.LogFormat("Sending {0} actions", actionsToBeSent.Count);
					client.SendActions(actionsToBeSent);
				}
			}

			if (actionsReceived != null)
			{
				Debug.LogFormat("Received {0} actions", actionsReceived.Count);
				foreach (var action in actionsReceived)
				{
					var entity = actionsContext.CreateEntity();

					if (action == null)
					{
						throw new ArgumentException("Actions must not be null");
					}
					entity.AddAction(action);
				}
			}
		}
	}
}