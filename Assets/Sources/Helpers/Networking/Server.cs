namespace Assets.Sources.Helpers.Networking
{
	using System.Collections.Generic;
	using ControlMessages;
	using UnityEngine;

	public class Server : NetworkEntity
	{
		private Dictionary<int, Player> players = new Dictionary<int, Player>();

		public Server()
		{
			RegisterHandler(typeof(ConnectMessage), HandleConnect);
		}

		public void Connect(string address, int port, string name)
		{
			Connect(address, port, name, true);

			Player = new Player(9999, name); // TODO: pretty ugly
			Players.AddPlayer(Player); 
			Debug.LogFormat("Server running on {0}:{1}", Address, Port);
		}

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

		public void SendToAll(IControlMessage message)
		{
			foreach (var player in Players)
			{
				SendTo(player, message);
			}
		}

		public override void HandleConnect(NetworkData data)
		{
			players.Add(data.ConnectionId, new Player(data.ConnectionId, "Unknown player"));
		}

		public override void HandleDisconnect(NetworkData data)
		{
			var player = Players.GetPlayer(data.ConnectionId);
			Players.RemovePlayer(player);

			var message = new DisconnectedMessage() { Player = player };
			SendToAll(message);
		}

		public override void HandleData(NetworkData data, IControlMessage message)
		{

		}

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
