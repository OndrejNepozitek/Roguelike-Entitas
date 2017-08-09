using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.Helpers.Networking
{
	using System.IO;
	using ControlMessages;
	using ProtoBuf;
	using UnityEngine;
	using UnityEngine.Networking;

	public abstract class NetworkEntity
	{
		protected const int DefaultPort = 5555;
		protected const string DefaultAddress = "127.0.0.1";

		protected Player Player;

		protected int HostId;
		protected int ConnectionId;

		protected int ControlChannel;
		protected int ActionsChannel;

		protected byte Error;

		protected string Address;
		protected int Port;

		protected Dictionary<Type, Action<IControlMessage, Player>> Handlers = new Dictionary<Type, Action<IControlMessage, Player>>();

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

		public void WatchNetwork()
		{
			int recHostId;
			int connectionId;
			int channelId;
			var recBuffer = new byte[1024];
			var bufferSize = 1024;
			int dataSize;
			byte error;
			NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

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
				case NetworkEventType.Nothing:         //1
					break;
				case NetworkEventType.ConnectEvent:    //2
					HandleConnect(data);
					break;
				case NetworkEventType.DataEvent:       //3
					HandleData(data);
					break;
				case NetworkEventType.DisconnectEvent: //4
					break;
			}

			if (recData != NetworkEventType.Nothing)
			{
				Debug.Log(recData.ToString());
			}


		}

		public void SendMessage(IControlMessage message, int connectionId)
		{
			var wrap = new ControlMessageWrap(message);
			var stream = new MemoryStream();
			Serializer.Serialize(stream, wrap);
			var byteArr = stream.ToArray();

			NetworkTransport.Send(HostId, connectionId, ControlChannel, byteArr, byteArr.Length, out Error);
		}

		public abstract void HandleConnect(NetworkData data);

		public abstract void HandleData(NetworkData data, IControlMessage message);

		public void HandleData(NetworkData data)
		{
			var message = DecodeMessage(data);
			Debug.Log(message.GetType());

			HandleData(data, message);

			Action<IControlMessage, Player> handler;
			if (Handlers.TryGetValue(message.GetType(), out handler))
			{
				if (handler != null)
				{
					handler(message, null);
				}
			}
		}

		protected IControlMessage DecodeMessage(NetworkData data)
		{
			var stream = new MemoryStream(data.RecBuffer, 0, data.DataSize);
			var wrap = Serializer.Deserialize<ControlMessageWrap>(stream);

			return wrap.Message;
		}

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
