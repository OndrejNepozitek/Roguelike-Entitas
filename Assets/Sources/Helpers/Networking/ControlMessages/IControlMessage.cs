namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	[ProtoInclude(1, typeof(SetNameMessage))]
	[ProtoInclude(2, typeof(WelcomeMessage))]
	[ProtoInclude(3, typeof(ConnectMessage))]
	[ProtoInclude(4, typeof(ConnectedMessage))]
	[ProtoInclude(5, typeof(DisconnectedMessage))]
	[ProtoInclude(6, typeof(StartGameMessage))]
	[ProtoInclude(7, typeof(GameStateMessage))]
	public interface IControlMessage
	{
	}
}