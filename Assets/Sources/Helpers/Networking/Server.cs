namespace Assets.Sources.Helpers.Networking
{
	using System.Collections.Generic;
	using ControlMessages;
	using Features.Actions;
	using UnityEngine;
	using UnityEngine.Networking;

	public class Server : NetworkEntity
	{
		public Server()
		{
			RegisterHandler(typeof(ConnectMessage), HandleConnect);
		}

		/// <summary>
		/// Create server
		/// </summary>
		/// <param name="address"></param>
		/// <param name="port"></param>
		/// <param name="name"></param>
		public void Connect(string address, int port, string name)
		{
			Connect(address, port, name, true);

			Player = new Player(9999, name); // TODO: pretty ugly
			Players.AddPlayer(Player); 
			Debug.LogFormat("Server running on {0}:{1}", Address, Port);
		}

		/// <summary>
		/// Send message to player
		/// </summary>
		/// <param name="player"></param>
		/// <param name="message"></param>
		public void SendTo(Player player, IControlMessage message)
		{
			if (player.Equals(Player))
			{
				TriggerHandlers(player, message);
			}
			else
			{
				SendMessage(message, player.Id);
			}
		}

		/// <summary>
		/// Send message to all players
		/// </summary>
		/// <param name="message"></param>
		public override void SendMessage(IControlMessage message)
		{
			foreach (var player in Players)
			{
				SendTo(player, message);
			}
		}

		/// <summary>
		/// Close server socket
		/// </summary>
		public override void Disconnect()
		{
			foreach (var player in Players)
			{
				NetworkTransport.Disconnect(HostId, player.Id, out Error);
			}

			NetworkTransport.RemoveHost(HostId);
		}

		/// <summary>
		/// Handle connect
		/// </summary>
		/// <param name="data"></param>
		public override void HandleConnect(NetworkData data)
		{
			// Players.Add(data.ConnectionId, new Player(data.ConnectionId, "Unknown player"));
		}

		/// <summary>
		/// Handle players disconnecting
		/// </summary>
		/// <param name="data"></param>
		public override void HandleDisconnect(NetworkData data)
		{
			var player = Players.GetPlayer(data.ConnectionId);
			Players.RemovePlayer(player);

			var message = new DisconnectedMessage() { Player = player };
			SendMessage(message);
		}

		/// <summary>
		/// Handle received data
		/// </summary>
		/// <param name="data"></param>
		/// <param name="message"></param>
		public override void HandleData(NetworkData data, IControlMessage message)
		{

		}
		
		/// <summary>
		/// Send actions
		/// </summary>
		/// <param name="actions"></param>
		public override void SendActions(List<IAction> actions)
		{
			foreach (var player in Players)
			{
				if (!player.Equals(Player))
				{
					SendMessage(actions, player.Id);
				}
			}
		}

		/// <summary>
		/// Handle players connecting
		/// </summary>
		/// <param name="rawMessage"></param>
		/// <param name="player"></param>
		private void HandleConnect(IControlMessage rawMessage, Player player)
		{
			var message = rawMessage as ConnectMessage;
			var newPlayer = new Player(player.Id, message.Name);
			
			var connectedMessage = new ConnectedMessage() { Player = newPlayer };
			Players.AddPlayer(player);
			foreach (var pl in Players)
			{
				if (pl.Equals(newPlayer))
				{
					continue;
				}

				SendTo(pl, connectedMessage);
			}

			var welcomeMessage = new WelcomeMessage() { Player = newPlayer, Players = Players };
			SendTo(newPlayer, welcomeMessage);
		}
	}
}
