namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	[ProtoInclude(1, typeof(SetNameMessage))]
	[ProtoInclude(2, typeof(WelcomeMessage))]
	[ProtoInclude(3, typeof(ConnectMessage))]
	public interface IControlMessage
	{
	}
}
