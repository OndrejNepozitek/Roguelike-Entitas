namespace Assets.Sources.Helpers.Networking
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using ActionMessages;
	using ControlMessages;
	using Features.Actions;
	using ProtoBuf;
	using UnityEngine;
	using UnityEngine.Networking;

	/// <summary>
	/// Common ancestor of both Client and Server.
	/// Its goal is to provide common api but still keep some differencies.
	/// </summary>
	public abstract class NetworkEntity
	{
		#region Fields

		/// <summary>
		/// Default port used for hosting/joining a game.
		/// </summary>
		protected const int DefaultPort = 5555;

		/// <summary>
		/// Default address used for hosting/joining a game.
		/// </summary>
		protected const string DefaultAddress = "127.0.0.1";

		/// <summary>
		/// Current player.
		/// </summary>
		public Player Player;

		/// <summary>
		/// List of currently connected players.
		/// </summary>
		public PlayersList Players = new PlayersList();

		protected int HostId;
		protected int ConnectionId;

		protected int ControlChannel;
		protected int ActionsChannel;

		protected byte Error;

		/// <summary>
		/// Current address.
		/// </summary>
		protected string Address;

		/// <summary>
		/// Current port.
		/// </summary>
		protected int Port;

		/// <summary>
		/// Dictionary of handlers for control messages.
		/// </summary>
		protected Dictionary<Type, Action<IControlMessage, Player>> Handlers = new Dictionary<Type, Action<IControlMessage, Player>>();

		public List<IAction> Actions;

		#endregion

		#region Constructor

		/// <summary>
		/// Class constructor.
		/// </summary>
		protected NetworkEntity()
		{
			RegisterInternalHandlers();
		}

		#endregion

		#region Abstract methods

		public abstract void HandleConnect(NetworkData data);

		public abstract void HandleDisconnect(NetworkData data);

		public abstract void HandleData(NetworkData data, IControlMessage message);

		public abstract void SendActions(List<IAction> actins);

		public abstract void SendMessage(IControlMessage message);

		public abstract void Disconnect();

		#endregion

		#region Internal handlers

		/// <summary>
		/// Register all internal handlers.
		/// </summary>
		private void RegisterInternalHandlers()
		{
			RegisterHandler<WelcomeMessage>(HandleWelcome);
			RegisterHandler<ConnectedMessage>(HandleConnected);
		}

		/// <summary>
		/// Handle welcome message.
		/// Set current player and update players list.
		/// </summary>
		private void HandleWelcome(WelcomeMessage message, Player player)
		{
			Player = message.Player;
			Players = message.Players;
		}

		/// <summary>
		/// Handle connected message.
		/// Add connected player to players list.
		/// </summary>
		private void HandleConnected(ConnectedMessage message, Player player)
		{
			Players.AddPlayer(message.Player);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Method for creating a host or joining one.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="port"></param>
		/// <param name="name"></param>
		/// <param name="isServer"></param>
		protected void Connect(string address, int port, string name, bool isServer)
		{
			NetworkTransport.Init();
			Address = string.IsNullOrEmpty(address) ? DefaultAddress : address;
			Port = port == 0 ? DefaultPort : port;

			var config = new ConnectionConfig();
			ControlChannel = config.AddChannel(QosType.ReliableSequenced);
			ActionsChannel = config.AddChannel(QosType.ReliableSequenced);

			var topology = new HostTopology(config, 10);
			HostId = NetworkTransport.AddHost(topology, isServer ? Port : 0);
		}

		/// <summary>
		/// Register handler for given message type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="handler"></param>
		public void RegisterHandler(Type type, Action<IControlMessage, Player> handler)
		{
			if (Handlers.ContainsKey(type))
			{
				Handlers[type] += handler;
			}
			else
			{
				Handlers.Add(type, handler);
			}
		}

		/// <summary>
		/// Register handler for given message type.
		/// </summary>
		/// <param name="handler"></param>
		public void RegisterHandler<T>(Action<T, Player> handler) where T : IControlMessage
		{
			Action<IControlMessage, Player> action = (message, player) =>
			{
				handler((T) message, player);
			};


			if (Handlers.ContainsKey(typeof(T)))
			{
				Handlers[typeof(T)] += action;
			}
			else
			{
				Handlers.Add(typeof(T), action);
			}
		}

		/// <summary>
		/// Watch network for events.
		/// TODO: refactor
		/// </summary>
		public void WatchNetwork()
		{
			Actions = null;

			int recHostId;
			int connectionId;
			int channelId;
			var recBuffer = new byte[1024];
			var bufferSize = 1024;
			int dataSize;
			byte error;

			// Call Unity's LLAPI function
			var recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

			var data = new NetworkData()
			{
				RecHostId = recHostId,
				ConnectionId = connectionId,
				ChannelId = channelId,
				RecBuffer = recBuffer,
				BufferSize = bufferSize,
				DataSize = dataSize,
				Error = error
			};

			switch (recData)
			{
				case NetworkEventType.ConnectEvent:
					HandleConnect(data);
					break;
				case NetworkEventType.DataEvent:
					if (data.ChannelId == ControlChannel)
					{
						HandleData(data);
					}
					else
					{
						Actions = DecodeActions(data);
					}
					break;
				case NetworkEventType.DisconnectEvent:
					HandleDisconnect(data);
					break;
			}
		}

		/// <summary>
		/// Send message to a player with given id.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="connectionId"></param>
		public void SendMessage(IControlMessage message, int connectionId)
		{
			var wrap = new ControlMessageWrap(message);
			var stream = new MemoryStream();
			Serializer.Serialize(stream, wrap);
			var byteArr = stream.ToArray();

			NetworkTransport.Send(HostId, connectionId, ControlChannel, byteArr, byteArr.Length, out Error);
		}

		/// <summary>
		/// Send message to a player with given id.
		/// </summary>
		/// <param name="actions"></param>
		/// <param name="connectionId"></param>
		public void SendMessage(List<IAction> actions, int connectionId)
		{
			var wrap = new ActionMessageWrap(actions);
			var stream = new MemoryStream();
			Serializer.Serialize(stream, wrap);
			var byteArr = stream.ToArray();

			NetworkTransport.Send(HostId, connectionId, ActionsChannel, byteArr, byteArr.Length, out Error);
		}

		/// <summary>
		/// Decode the message and trigger handlers.
		/// </summary>
		/// <param name="data"></param>
		public void HandleData(NetworkData data)
		{
			var message = DecodeMessage(data);

			HandleData(data, message);
			TriggerHandlers(data, message);
		}

		/// <summary>
		/// Trigger all registered handlers for given message type.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="message"></param>
		protected void TriggerHandlers(NetworkData data, IControlMessage message)
		{
			var player = Players.GetPlayer(data.ConnectionId) ?? new Player(data.ConnectionId, null);

			TriggerHandlers(player, message);
		}

		/// <summary>
		/// Trigger all registered handlers for given message type.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="message"></param>
		protected void TriggerHandlers(Player player, IControlMessage message)
		{
			Debug.Log("Triggering handlers for " + message.GetType().Name);

			Action<IControlMessage, Player> handler;
			if (Handlers.TryGetValue(message.GetType(), out handler))
			{
				if (handler != null)
				{
					handler(message, player);
				}
			}
		}

		/// <summary>
		/// Decode a message using protobuf-net.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		protected IControlMessage DecodeMessage(NetworkData data)
		{
			var stream = new MemoryStream(data.RecBuffer, 0, data.DataSize);
			var wrap = Serializer.Deserialize<ControlMessageWrap>(stream);

			return wrap.Message;
		}

		protected List<IAction> DecodeActions(NetworkData data)
		{
			var stream = new MemoryStream(data.RecBuffer, 0, data.DataSize);
			var wrap = Serializer.Deserialize<ActionMessageWrap>(stream);

			if (wrap.Actions.Count > 1)
			{
				Debug.Log(string.Format("Decoded {0}", wrap.Actions.Count));
			}
			else
			{
				Debug.Log(string.Format("Decoded 1 action - {0}", wrap.Actions[0].GetType().Name));
			}
			

			return wrap.Actions;
		}

		#endregion

		public class NetworkData
		{
			public int RecHostId;
			public int ConnectionId;
			public int ChannelId;
			public byte[] RecBuffer = new byte[1024];
			public int BufferSize = 1024;
			public int DataSize;
			public byte Error;
		}
	}
}
