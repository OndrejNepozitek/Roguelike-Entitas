namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	internal class GameStateMessage : IControlMessage
	{
		[ProtoMember(1)] public GameState State;
	}
}