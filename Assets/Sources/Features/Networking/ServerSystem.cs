using System;
using System.Linq;
using Assets.Sources.Helpers.Networking;
using Entitas;

public class ServerSystem : IExecuteSystem, ICleanupSystem
{
	private readonly ActionsContext actionsContext;

	public ServerSystem(Contexts contexts)
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

	public void Cleanup()
	{
		var server = NetworkController.Instance.NetworkEntity as Server; // TODO: ugly, slow
		if (server != null)
		{
			var actions = actionsContext.GetEntities().Where(e => e.hasAction).Select(e => e.action.Action).ToList();

			if (actions.Count != 0)
			{
				server.SendActions(actions);
			}
		}
	}
}