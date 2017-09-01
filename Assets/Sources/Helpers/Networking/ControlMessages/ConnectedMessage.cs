namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	public class ConnectedMessage : IControlMessage
	{
		[ProtoMember(1)] public Player Player;
	}
}