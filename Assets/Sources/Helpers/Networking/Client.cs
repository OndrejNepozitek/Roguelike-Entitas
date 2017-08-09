namespace Assets.Sources.Helpers.Networking
{
	using System.IO;
	using ControlMessages;
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

		public override void HandleData(NetworkData data, IControlMessage message)
		{
			throw new System.NotImplementedException();
		}

		public void SendMessage(IControlMessage message)
		{
			SendMessage(message, ConnectionId);
		}
	}
}
