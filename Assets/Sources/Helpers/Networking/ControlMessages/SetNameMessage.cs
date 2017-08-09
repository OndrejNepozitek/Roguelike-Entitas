namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	public class SetNameMessage : IControlMessage
	{
		[ProtoMember(1)]
		public string Name { get; set; }
	}
}
