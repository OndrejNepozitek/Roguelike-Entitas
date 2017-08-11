namespace Assets.Sources.Helpers.Networking.ActionMessages
{
	using System.Collections.Generic;
	using ProtoBuf;

	[ProtoContract]
	public class ActionMessageWrap
	{
		public ActionMessageWrap()
		{
			
		}

		public ActionMessageWrap(List<IAction> actions)
		{
			Actions = actions;
		}

		[ProtoMember(1)]
		public List<IAction> Actions { get; set; }
	}
}