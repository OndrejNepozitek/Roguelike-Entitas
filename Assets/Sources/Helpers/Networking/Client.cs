namespace Assets.Sources.Helpers.Networking
{
	using System.Collections.Generic;
	using ControlMessages;
	using Features.Actions;
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

		/// <summary>
		/// Handle players disconnecting
		/// </summary>
		/// <param name="data"></param>
		public override void HandleDisconnect(NetworkData data)
		{	
			// TODO: this gets triggered when the client itself disconnects - why?
			TriggerHandlers(new Player(0, null), new HostDisconnectedMessage()); // TODO: improve
		}

		/// <summary>
		/// Handle incoming data
		/// </summary>
		/// <param name="data"></param>
		/// <param name="message"></param>
		public override void HandleData(NetworkData data, IControlMessage message)
		{
			
		}

		/// <summary>
		/// Send actions to server.
		/// </summary>
		/// <param name="actions"></param>
		public override void SendActions(List<IAction> actions)
		{
			SendMessage(actions, ConnectionId);
		}

		/// <summary>
		/// Send controll message to server
		/// </summary>
		/// <param name="message"></param>
		public override void SendMessage(IControlMessage message)
		{
			SendMessage(message, ConnectionId);
		}

		/// <summary>
		/// Close the socket.
		/// </summary>
		public override void Disconnect()
		{
			NetworkTransport.Disconnect(HostId, ConnectionId, out Error);
		}
	}
}
