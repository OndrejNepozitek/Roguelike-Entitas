namespace Assets.Sources.Features.Combat
{
	using Actions;
	using ProtoBuf;

	[ProtoContract]
	public class AttackAction : IAction
	{
		[ProtoMember(1)]
		public AttackType Type;

		[ProtoMember(2)]
		public EntityReference Source;

		[ProtoMember(3)]
		public EntityReference Target;

		[ProtoMember(4)]
		public float Value;

		public bool Validate(GameContext context)
		{
			return true;
		}
	}
}
