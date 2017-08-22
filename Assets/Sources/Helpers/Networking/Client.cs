namespace Assets.Sources.Helpers.Networking
{
	using System.Collections.Generic;
	using System.IO;
	using ControlMessages;
	using Features.Actions;
	using ProtoBuf;
	using UnityEngine;
	using UnityEngine.Networking;

	public class Client : NetworkEntity
	{
		public void Connect(string address, int port, string name)
		{
			Connect(address, port, name, false);
			ConnectionId = NetworkTransport.Connect(HostId, Address, Port, 0, out Error);
		}

		/// <summary>
		/// Send ConnectMessage when a connection is established.
		/// </summary>
		/// <param name="data"></param>
		public override void HandleConnect(NetworkData data)
		{
			Debug.Log("Sending ConnectMessage");
			SendMessage(new ConnectMessage() { Name = "Test" });
		}

		public override void HandleDisconnect(NetworkData data)
		{
			
		}

		public override void HandleData(NetworkData data, IControlMessage message)
		{
			
		}

		public override void SendActions(List<IAction> actions)
		{
			Debug.Log(string.Format("Sending {0} actions", actions.Count));
			SendMessage(actions, ConnectionId);
		}

		public override void SendMessage(IControlMessage message)
		{
			SendMessage(message, ConnectionId);
		}
	}
}
