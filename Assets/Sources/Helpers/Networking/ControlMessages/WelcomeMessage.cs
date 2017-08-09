using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Sources.Helpers.Networking.ControlMessages
{
	using ProtoBuf;

	[ProtoContract]
	public class WelcomeMessage : IControlMessage
	{
		[ProtoMember(1)]
		public int Id;

		[ProtoMember(2)]
		public Dictionary<int, Player> Players;
	}
}
