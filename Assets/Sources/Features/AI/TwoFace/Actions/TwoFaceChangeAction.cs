
namespace Assets.Sources.Features.AI.TwoFace.Actions
{
	using Features.Actions;
	using ProtoBuf;

	[ProtoContract]
	public class TwoFaceChangeAction : IAction
	{
		[ProtoMember(1)]
		public bool IsAngry;

		public bool Validate(GameContext context)
		{
			return true;
		}
	}
}
