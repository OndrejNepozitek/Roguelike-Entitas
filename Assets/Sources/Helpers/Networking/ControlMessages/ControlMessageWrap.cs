namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	public class ControlMessageWrap
	{
		public ControlMessageWrap()
		{
			
		}

		public ControlMessageWrap(IControlMessage message)
		{
			Message = message;
		}

		[ProtoMember(1)]
		public IControlMessage Message { get; set; }
	}
}