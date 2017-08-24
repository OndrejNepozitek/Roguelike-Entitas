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
			var entity = Source.GetEntity();

			// TODO: let player move when he is already moving for prediction
			// It should be somehow made to avoid cheating
			if (entity.isActionInProgress && (!entity.hasPlayer || entity.player.Focus)) return false;

			return true;
		}
	}
}
