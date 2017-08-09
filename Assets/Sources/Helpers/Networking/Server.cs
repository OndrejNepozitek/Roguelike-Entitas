namespace Assets.Sources.Helpers.Networking
{
	using System.Collections.Generic;
	using ControlMessages;
	using UnityEngine;
	using UnityEngine.Networking;

	public class Server : NetworkEntity
	{
		private Dictionary<int, Player> players = new Dictionary<int, Player>();

		public Server()
		{
			RegisterHandler(typeof(ConnectMessage), (message, player) => Debug.Log("This is ConnectMessage hook!"));
		}

		public void Connect(string address, int port, string name)
		{
			Connect(address, port, name, true);
			Debug.LogFormat("Server running on {0}:{1}", Address, Port);
		}

		public void SendTo(Player player, IControlMessage message)
		{
			
		}

		public override void HandleConnect(NetworkData data)
		{
			players.Add(data.ConnectionId, new Player(data.ConnectionId, "Unknown player"));
		}

		public override void HandleData(NetworkData data, IControlMessage message)
		{

		}
	}
}
