namespace Assets.Sources.Features.Stats
{
	using Actions;
	using Helpers.Networking;
	using ProtoBuf;

	[ProtoContract]
	public class RespawnAction : IAction
	{
		[ProtoMember(1)]
		public IntVector2 Position;

		[ProtoMember(2)]
		public EntityReference Entity;

		[ProtoMember(3)]
		public Player Player;

		public bool Validate(GameContext context)
		{
			return true;
		}
	}
}
