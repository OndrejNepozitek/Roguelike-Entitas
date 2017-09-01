namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	public class WelcomeMessage : IControlMessage
	{
		[ProtoMember(1)] public Player Player;

		[ProtoMember(2)] public PlayersList Players;
	}
}